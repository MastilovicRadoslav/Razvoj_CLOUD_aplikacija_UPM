using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Entities
{
    public class PostData : TableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string UserEmail { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }

        private static long _idCounter = 0;
        private static readonly object _idLock = new object();

        public PostData(string title)
        {
            PartitionKey = "Post";
            RowKey = GenerateNextId().ToString();
        }

        public PostData() { }

        public PostData(string title, string description, string image, string userEmail, int like, int unLike)
        {
            Id = (int)GenerateNextId();
            Title = title;
            Description = description;
            Image = image;
            UserEmail = userEmail;
            Like = like;
            UnLike = unLike;
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