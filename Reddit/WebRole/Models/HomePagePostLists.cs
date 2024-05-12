using System.Collections.Generic;

namespace Models
{
    public class HomePagePostLists
    {
        public List<Post> MyPosts { get; set; } = new List<Post>();
        public List<Post> AllPosts { get; set; } = new List<Post>();
    }
}