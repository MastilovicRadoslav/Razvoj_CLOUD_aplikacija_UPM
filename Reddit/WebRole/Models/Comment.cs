namespace Models
{
    public class Comment
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public string UserEmail { get; set; }

        public Comment()
        {
        }

        public Comment(string text, int postId, string userEmail)
        {
            Text = text;
            PostId = postId;
            UserEmail = userEmail;
        }
    }
}