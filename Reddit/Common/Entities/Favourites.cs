using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Entities
{
    public class Favourites : TableEntity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserEmail { get; set; }

        private static long _idCounter = 0;
        private static readonly object _idLock = new object();

        public Favourites(int postId)
        {
            PartitionKey = "Favourites";
            RowKey = GenerateNextId().ToString();
        }

        public Favourites() { }

        public Favourites(int postId, string userEmail)
        {
            Id = (int)GenerateNextId();
            PostId = postId;
            UserEmail = userEmail;
        }

        private static long GenerateNextId()
        {
            lock (_idLock)
            {
                return _idCounter++;
            }
        }
    }
}