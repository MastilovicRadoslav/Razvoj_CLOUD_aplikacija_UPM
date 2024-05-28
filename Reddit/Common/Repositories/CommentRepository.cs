using Common.Entities;
using Common.Interfaces;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

namespace Common.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTable _table;

        public CommentRepository()
        {
            string connectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string for Azure Storage is not set.");
            }

            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("CommentTableTemp");
            _table.CreateIfNotExists();
        }

        public IQueryable<CommentData> ReadAll()
        {
            try
            {
                var results = from g in _table.CreateQuery<CommentData>()
                              where g.PartitionKey == "Comment"
                              select g;
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}");
            }
        }

        public void Create(CommentData comment)
        {
            try
            {
                TableOperation insertOperation = TableOperation.Insert(comment);
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

        public CommentData Read(int id)
        {
            return ReadAll().Where(p => p.RowKey == id.ToString()).FirstOrDefault();
        }
    }
}