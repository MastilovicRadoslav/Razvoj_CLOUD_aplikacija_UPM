using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Entities
{
    public class CommentData : TableEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public string UserEmail { get; set; }

        private static long _idCounter = 0;
        private static readonly object _idLock = new object();

        public CommentData(string text)
        {
            PartitionKey = "Comment";
            RowKey = GenerateNextId().ToString();
        }

        public CommentData() { }

        public CommentData(string text, int postId, string userEail)
        {
            Id = (int)GenerateNextId();
            Text = text;
            PostId = postId;
            UserEmail = userEail;
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