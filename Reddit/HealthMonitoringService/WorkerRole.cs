using Common.Entities;
using Common.Interfaes;
using Common.Repositories;
using Microsoft.WindowsAzure.ServiceRuntime;
using Polly;
using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitoringService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private readonly HealthStatusRepository healthStatusRepo = new HealthStatusRepository();
        private ICheckServiceStatus serviceRedditProxy;
        private ICheckServiceStatus serviceNotificationProxy;

        public override void Run()
        {
            Trace.TraceInformation("HealthMonitoringService is running");

            try
            {
                this.RunWithRetryAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        private async Task RunWithRetryAsync(CancellationToken token)
        {

            var retryPolicy = Policy
                .Handle<CommunicationException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Trace.WriteLine($"Retry {retryCount} encountered a {exception.GetType().Name}. Waiting {timeSpan} before next retry. Exception: {exception.Message}");
                    });


            await retryPolicy.ExecuteAsync(async () =>
            {
                await RunAsync(token);
            });
        }

        private async Task MonitorWebRoleStatusAsync()
        {
            bool serviceRedditAvailable = serviceRedditProxy.CheckServiceStatus();
            if (serviceRedditAvailable)
            {
                Trace.WriteLine("RedditService is running!");
                healthStatusRepo.Create(new HealthStatus("RedditService") { ServiceType = "RedditService", Status = "OK" });
            }
            bool notificationServicaAvailable = serviceNotificationProxy.CheckServiceStatus();
            if (notificationServicaAvailable)
            {
                Trace.WriteLine("NotificationService is running!");
                healthStatusRepo.Create(new HealthStatus("NotificationService") { ServiceType = "NotificationService", Status = "OK" });
            }

            await Task.Delay(5000);
        }

        public override bool OnStart()
        {
            // Use TLS 1.2 for Service Bus connections
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            ConnecToReddit();
            ConnecToNotificationService();

            bool result = base.OnStart();

            Trace.TraceInformation("HealthMonitoringService has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("HealthMonitoringService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("HealthMonitoringService has stopped");
        }

        private async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    EnsureConnection();

                    await MonitorWebRoleStatusAsync();
                }
                catch (CommunicationException ex)
                {
                    Trace.WriteLine($"CommunicationException: {ex.Message}");
                    throw;
                }
                catch (TimeoutException ex)
                {
                    Trace.WriteLine($"TimeoutException: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Unexpected exception: {ex.Message}");
                    throw;
                }
            }
        }

        private void EnsureConnection()
        {
            if (serviceRedditProxy == null)
            {
                ConnecToReddit();
            }
            try
            {
                serviceRedditProxy.CheckServiceStatus();
            }
            catch (CommunicationException ex)
            {
                Trace.WriteLine($"Reddit is down! {ex} \n Reconnecting...");
                healthStatusRepo.Create(new HealthStatus("RedditService") { ServiceType = "RedditService", Status = "NOT_OK" });
                ConnecToReddit();
            }

            if (serviceNotificationProxy == null)
            {
                ConnecToNotificationService();
            }
            try
            {
                serviceNotificationProxy.CheckServiceStatus();
            }
            catch (CommunicationException ex)
            {
                Trace.WriteLine($"Notification is down! {ex} \n Reconnecting...");
                healthStatusRepo.Create(new HealthStatus("NotificationService") { ServiceType = "NotificationService", Status = "NOT_OK" });
                ConnecToNotificationService();
            }
        }

        public void ConnecToReddit()
        {
            var endpoint = RoleEnvironment.Roles["RedditService"].Instances[0].InstanceEndpoints["HealthCheck"];
            var address = new EndpointAddress($"net.tcp://{endpoint.IPEndpoint}/Service");
            var binding = new NetTcpBinding();
            ChannelFactory<ICheckServiceStatus> factory = new ChannelFactory<ICheckServiceStatus>(binding, address);
            serviceRedditProxy = factory.CreateChannel();

        }

        public void ConnecToNotificationService()
        {
            var binding = new NetTcpBinding();
            var endpoint = RoleEnvironment.Roles["NotificationService"].Instances[0].InstanceEndpoints["HealthCheck"];
            var address = new EndpointAddress($"net.tcp://{endpoint.IPEndpoint.Address}:{endpoint.IPEndpoint.Port}/Service");
            ChannelFactory<ICheckServiceStatus> factory = new ChannelFactory<ICheckServiceStatus>(binding, address);
            serviceNotificationProxy = factory.CreateChannel();
        }
    }
}