using Common.Interfaes;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace RedditService
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        ServiceHost serviceHost;
        private readonly string internalEndpointName = "HealthCheck";
        private readonly static UserServer UserServer = new UserServer();
        private readonly static PostServer PostServer = new PostServer();
        private readonly static CommentServer CommentServer = new CommentServer();
        private readonly static FavouritesServer FavouritesServer = new FavouritesServer();

        public override void Run()
        {
            Trace.TraceInformation("RedditService is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
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
                serviceHost = new ServiceHost(typeof(ServiceStatus));
                NetTcpBinding binding = new NetTcpBinding();
                serviceHost.AddServiceEndpoint(typeof(ICheckServiceStatus), binding, endpointAddress);
                serviceHost.Open();
                Trace.WriteLine("Communication established!");
            }
            catch
            {
                serviceHost.Abort();
            }

            bool result = base.OnStart();
            UserServer.Open();
            PostServer.Open();
            CommentServer.Open();
            FavouritesServer.Open();

            Trace.TraceInformation("RedditService has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("RedditService is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();
            UserServer.Close();
            PostServer.Close();
            CommentServer.Close();
            FavouritesServer.Close();

            Trace.TraceInformation("RedditService has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}