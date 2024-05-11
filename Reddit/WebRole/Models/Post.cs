using System.Collections.Generic;

namespace Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<Comment> Comments { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }

        public Post()
        {
            Comments = new List<Comment>();
        }
    }
}