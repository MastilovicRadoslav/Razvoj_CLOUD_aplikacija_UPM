using Common.Comment_queue;
using Common.Entities;
using Common.Interfaes;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using NotificationService.Notification_via_email;
using RedditService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        ServiceHost serviceHost;
        private readonly string internalEndpointName = "HealthCheck";
        private readonly FavouritesServiceProvider favouritesService = new FavouritesServiceProvider();
        private readonly EmailSender emailSender = new EmailSender();

        public override void Run()
        {
            Trace.TraceInformation("NotificationService is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Use TLS 1.2 for Service Bus connections
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            try
            {
                var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[internalEndpointName];
                var endpointAddress = $"net.tcp://{endpoint.IPEndpoint}/Service";
                serviceHost = new ServiceHost(typeof(ServiceStatus), new Uri(endpointAddress));
                NetTcpBinding binding = new NetTcpBinding();
                serviceHost.AddServiceEndpoint(typeof(ICheckServiceStatus), binding, endpointAddress);
                serviceHost.Open();
                Trace.TraceInformation("Communication established!");
            }
            catch
            {
                serviceHost.Abort();
            }

            bool result = base.OnStart();

            Trace.TraceInformation("NotificationService has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("NotificationService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("NotificationService has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);

                CloudQueue queue = QueueHelper.GetQueueReference("comments");
                while (true)
                {
                    CloudQueueMessage message = queue.GetMessage();
                    if (message == null)
                    {
                        Trace.TraceInformation("Trenutno ne postoji poruka u redu.");
                    }
                    else
                    {
                        Trace.TraceInformation(String.Format("Poruka glasi: {0}", message.AsString));

                        List<string> emails = new List<string>();
                        CommentData commentData = CommentData.FromString(message.AsString);
                        List<Favourites> allFavouritesFromTable = favouritesService.GetAllFavourites();
                        foreach (var favourites in allFavouritesFromTable)
                        {
                            if (favourites.PostId == commentData.PostId)
                            {
                                emails.Add(favourites.UserEmail);
                            }
                        }

                        if (emails.Count != 0)
                        {
                            string subject = "Postavljen je novi komentar na temu na koju ste pretplaæeni";
                            StringBuilder bodyBuilder = new StringBuilder();
                            foreach (var email in emails)
                            {
                                bodyBuilder.AppendLine(email);
                            }
                            bodyBuilder.AppendLine();
                            bodyBuilder.AppendLine("Text komentara koji je postavljen: " + commentData.Text);
                            string body = bodyBuilder.ToString().TrimEnd();
                            body = body.Replace(Environment.NewLine, "<br/>");
                            emailSender.SendEmail("drsprojekat2023@gmail.com", subject, body);
                        }

                        Trace.TraceInformation("Informacije o poslatoj grupi notifikacija: ");
                        Trace.TraceInformation("|Datum i vreme: " + DateTime.Now + "|ID komentara: " + commentData.Id + "|Broj poslatih mejlova: " + emails.Count + "|");

                        if (message.DequeueCount >= 2)
                        {
                            queue.DeleteMessage(message);
                        }
                    }
                    Thread.Sleep(5000);
                }
            }
        }
    }
}