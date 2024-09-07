namespace Models
{
    public class Comment
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public string UserEmail { get; set; }
        public int Like { get; set; }
        public int Unlike { get; set; }

        public Comment()
        {
        }

        public Comment(string text, int postId, string userEmail, int like, int unlike)
        {
            Text = text;
            PostId = postId;
            UserEmail = userEmail;
            Like = like;
            Unlike = unlike;

        }
    }
}