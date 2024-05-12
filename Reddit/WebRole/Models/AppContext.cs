using System.Collections.Generic;

namespace Models
{
    public static class AppContext
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static HomePagePostLists homePagePostLists = new HomePagePostLists();
    }
}