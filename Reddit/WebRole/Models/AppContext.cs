using FrontReddit.Models;
using System.Collections.Generic;

namespace FrontReddit
{
    public static class AppContext
    {
        public static List<User> Users { get; set; } = new List<User>();
    }
}