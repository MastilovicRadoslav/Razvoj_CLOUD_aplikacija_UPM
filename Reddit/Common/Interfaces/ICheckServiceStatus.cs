using System.ServiceModel;

namespace Common.Interfaes
{
    [ServiceContract]
    public interface ICheckServiceStatus
    {
        [OperationContract]
        bool CheckServiceStatus();
    }
}