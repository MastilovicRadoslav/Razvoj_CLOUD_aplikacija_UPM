using Common.Entities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Common.Repositories
{
    public class HealthStatusRepository
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTable _table;

        public HealthStatusRepository()
        {
            string connectionString = CloudConfigurationManager.GetSetting("HealthStatusConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string for Azure Storage is not set.");
            }

            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("HealthStatusConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("HealthStatusTableTemp");
            _table.CreateIfNotExists();
        }

        public void Create(HealthStatus newHealthStatus)
        {
            try
            {
                TableOperation insertOperation = TableOperation.Insert(newHealthStatus);
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
    }
}