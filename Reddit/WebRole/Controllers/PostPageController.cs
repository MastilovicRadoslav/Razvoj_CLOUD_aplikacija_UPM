using Models;
using System.Linq;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class PostPageController : Controller
    {
        // GET: PostPage
        // Prikaz odabranog posta na osnovu IDa
        public ActionResult PostPage(int postId)
        {
            Post post = AppContext.homePagePostLists.AllPosts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                return View(post);
            }
            return View();
        }
    }
}