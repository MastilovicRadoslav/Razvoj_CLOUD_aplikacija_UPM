using Common.Entities;
using System.Linq;

namespace Common.Interfaces
{
    public interface IUserRepository
    {
        void Create(UserData user);
        UserData Read(string email);
        IQueryable<UserData> ReadAll();
        void Update(string email, UserData user);
        bool Exists(string email);
    }
}