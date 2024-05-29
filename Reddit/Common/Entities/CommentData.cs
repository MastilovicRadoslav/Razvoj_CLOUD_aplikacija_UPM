using Microsoft.WindowsAzure.Storage.Table;
using System;

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

        public override string ToString()
        {
            return $"Komentar: ID komentara: {Id}, Text: {Text}, ID posta: {PostId}, Email korisnika koji je postavio komentar: {UserEmail}";
        }

        public static CommentData FromString(string message)
        {
            var parts = message.Split(new[] { ", " }, StringSplitOptions.None);
            var idPart = parts[0].Split(new[] { ": " }, StringSplitOptions.None)[2];
            var textPart = parts[1].Split(new[] { ": " }, StringSplitOptions.None)[1];
            var postIdPart = parts[2].Split(new[] { ": " }, StringSplitOptions.None)[1];
            var emailPart = parts[3].Split(new[] { ": " }, StringSplitOptions.None)[1];

            int id = int.Parse(idPart);
            string text = textPart;
            int postId = int.Parse(postIdPart);
            string userEmail = emailPart;

            var commentData = new CommentData(text, postId, userEmail)
            {
                Id = id,
                Text = text,
                PostId = postId,
                UserEmail = userEmail
            };
            return commentData;
        }
    }
}