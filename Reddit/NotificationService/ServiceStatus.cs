using Common.Interfaes;

namespace NotificationService
{
    public class ServiceStatus : ICheckServiceStatus
    {
        public bool CheckServiceStatus()
        {
            return true;
        }
    }
}