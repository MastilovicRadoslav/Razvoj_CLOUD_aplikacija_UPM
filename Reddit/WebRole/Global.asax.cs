using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebRole
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            InitBlobs();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void InitBlobs()
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BlobDataConnectionString"));
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer containerProfile = blobStorage.GetContainerReference("redditprofileimages");
                CloudBlobContainer containerPost = blobStorage.GetContainerReference("redditpostimages");
                containerProfile.CreateIfNotExists();
                containerPost.CreateIfNotExists();
                var permissionsProfile = containerProfile.GetPermissions();
                var permissionsPost = containerPost.GetPermissions();
                permissionsProfile.PublicAccess = BlobContainerPublicAccessType.Container;
                permissionsPost.PublicAccess = BlobContainerPublicAccessType.Container;
                containerProfile.SetPermissions(permissionsProfile);
                containerPost.SetPermissions(permissionsPost);
            }
            catch (WebException ex)
            {
                throw new WebException($"{ex}");
            }
        }
    }
}