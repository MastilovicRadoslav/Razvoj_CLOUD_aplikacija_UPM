using Common.Entities;
using Common.Interfaces;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

namespace Common.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTable _table;

        public UserRepository()
        {
            string connectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string for Azure Storage is not set.");
            }

            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("UserTableTemp");
            _table.CreateIfNotExists();
        }

        public IQueryable<UserData> ReadAll()
        {
            try
            {
                var results = from g in _table.CreateQuery<UserData>()
                              where g.PartitionKey == "User"
                              select g;
                return results;
            }
            catch (Exception ex)
            {
               
                throw new Exception($"{ex}");
            }
        }

        public void Create(UserData newUser)
        {
            try
            {
                TableOperation insertOperation = TableOperation.Insert(newUser);
                TableResult result = _table.Execute(insertOperation);

                if (result.HttpStatusCode < 200 || result.HttpStatusCode >= 300)
                {
                    throw new InvalidOperationException("Failed to insert user into Azure Table Storage.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex}");
            }
        }

        public bool Exists(string email)
        {
            return ReadAll().Where(s => s.RowKey == email).FirstOrDefault() != null;
        }

        public UserData Read(string email)
        {
            return ReadAll().Where(p => p.RowKey == email).FirstOrDefault();
        }

        public void Update(string email, UserData user)
        {
            if (string.IsNullOrEmpty(email) || user == null)
                return;

            try
            {
                var existingUser = (from g in _table.CreateQuery<UserData>()
                                    where g.PartitionKey == "User" && g.RowKey == email
                                    select g).FirstOrDefault();

                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Address = user.Address;
                    existingUser.City = user.City;
                    existingUser.Country = user.Country;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Email = user.Email;
                    existingUser.Password = user.Password;
                    existingUser.Image = user.Image;
                    TableOperation updateOperation = TableOperation.Replace(existingUser);
                    _table.Execute(updateOperation);
                    return;
                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}