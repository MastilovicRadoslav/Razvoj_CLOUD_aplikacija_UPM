namespace FrontReddit.Models
{
    public class Comment
    {
        public string Text { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }

        public Comment()
        {
        }
    }
}