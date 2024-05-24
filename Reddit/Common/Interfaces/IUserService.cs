using Common.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        void AddUser(UserData user);
        [OperationContract]
        List<UserData> GetAllUsers();
        [OperationContract]
        UserData GetUser(string email);
        [OperationContract]
        void UpdateUser(string email, UserData user);
        [OperationContract]
        bool Exists(string email);
    }
}