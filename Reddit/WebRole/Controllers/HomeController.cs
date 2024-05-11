using FrontReddit.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

namespace WebRole.Controllers
{
	public class HomeController : Controller
	{
        // GET: Home page
        // Prikaz početne stranice
        public ActionResult Index()
        {
            List<Post> posts = new List<Post>();

            Post p1 = new Post();
            p1.Title = "Naslov posta 1";
            p1.Description = "Neki kratak opis 1";
            p1.Image = "/Images/Backgrounds/profile.jpg";
            p1.Comments.Add(new Comment("Neki komentar 1", "neko2@gmail.com"));
            p1.UserEmail = "neko@gmail.com";
            p1.Like = 0;
            p1.UnLike = 0;
            posts.Add(p1);

            Post p2 = new Post();
            p2.Title = "Naslov posta 2";
            p2.Description = "Neki kratak opis 2";
            p2.Image = "";
            p2.Comments.Add(new Comment("Neki komentar 2", "neko2@gmail.com"));
            p2.UserEmail = "neko@gmail.com";
            p2.Like = 0;
            p2.UnLike = 0;
            posts.Add(p2);

            Post p3 = new Post();
            p3.Title = "Naslov posta 3";
            p3.Description = "Neki kratak opis 3";
            p3.Image = "";
            p3.Comments.Add(new Comment("Neki komentar 3", "neko2@gmail.com"));
            p3.UserEmail = "neko@gmail.com";
            p3.Like = 0;
            p3.UnLike = 0;
            posts.Add(p3);

            return View(posts);
        }
	}
}