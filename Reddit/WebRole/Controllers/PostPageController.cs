using Models;
using System.Linq;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class PostPageController : Controller
    {
        // GET: PostPage
        // Prikaz odabranog posta na osnovu naslova
        public ActionResult PostPage(string postTitle)
        {
            Post post = AppContext.homePagePostLists.AllPosts.FirstOrDefault(p => p.Title == postTitle);
            if(post != null)
            {
                return View(post);
            }
            return View();
        }
    }
}