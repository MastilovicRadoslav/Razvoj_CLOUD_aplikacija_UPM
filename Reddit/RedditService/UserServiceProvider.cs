using Common.Entities;
using Common.Interfaces;
using Common.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RedditService
{
    public class UserServiceProvider : IUserService
    {
        private readonly static UserRepository repository = new UserRepository();

        public void AddUser(UserData user)
        {
            repository.Create(new UserData(user.Email) { FirstName = user.FirstName, LastName = user.LastName, Address = user.Address, City = user.City, Country = user.Country, PhoneNumber = user.PhoneNumber, Email = user.Email, Password = user.Password, Image = user.Image });
        }

        public List<UserData> GetAllUsers()
        {
            return repository
                  .ReadAll()
                  .Select(user => new UserData
                  {
                      FirstName = user.FirstName,
                      LastName = user.LastName,
                      Address = user.Address,
                      City = user.City,
                      Country = user.Country,
                      PhoneNumber = user.PhoneNumber,
                      Email = user.Email,
                      Password = user.Password,
                      Image = user.Image
                  }).ToList();
        }

        public UserData GetUser(string email)
        {
            UserData user = repository.Read(email);
            return new UserData(user.FirstName, user.LastName, user.Address, user.City, user.Country, user.PhoneNumber, user.Email, user.Password, user.Image);
        }

        public void UpdateUser(string email, UserData user)
        {
            repository.Update(email, new UserData(user.FirstName, user.LastName, user.Address, user.City, user.Country, user.PhoneNumber, user.Email, user.Password, user.Image));
        }

        public bool Exists(string email)
        {
            return repository.Exists(email);
        }
    }
}