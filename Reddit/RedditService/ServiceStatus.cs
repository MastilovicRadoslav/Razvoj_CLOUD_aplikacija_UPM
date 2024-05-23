using Common.Interfaes;

namespace RedditService
{
    public class ServiceStatus : ICheckServiceStatus
    {
        public bool CheckServiceStatus()
        {
            return true;
        }
    }
}