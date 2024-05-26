using Common.Entities;
using Common.Interfaces;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

namespace Common.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTable _table;

        public PostRepository()
        {
            string connectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string for Azure Storage is not set.");
            }

            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("PostTableTemp");
            _table.CreateIfNotExists();
        }

        public IQueryable<PostData> ReadAll()
        {
            try
            {
                var results = from g in _table.CreateQuery<PostData>()
                              where g.PartitionKey == "Post"
                              select g;
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex}");
            }
        }

        public void Create(PostData post)
        {
            try
            {
                TableOperation insertOperation = TableOperation.Insert(post);
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

        public PostData Read(int id)
        {
            return ReadAll().Where(p => p.RowKey == id.ToString()).FirstOrDefault();
        }

        public bool Delete(int id)
        {
            if (id < 0)
                return false;

            var postToDelete = (from g in _table.CreateQuery<PostData>()
                                where g.PartitionKey == "Post" && g.RowKey == id.ToString()
                                select g).FirstOrDefault();

            if (postToDelete != null)
            {
                try
                {
                    TableOperation deleteOperation = TableOperation.Delete(postToDelete);
                    _table.Execute(deleteOperation);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception($"{ex}");
                }
            }
            else
            {
                return false;
            }
        }
    }
}