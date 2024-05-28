using Common.Entities;
using Common.Interfaces;
using Models;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using WebRole.UniversalConnector;

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

        // POST: PostPage/NewComment
        // Obrada zahteva za dodavanje novog komentara
        [HttpPost]
        public ActionResult NewComment(int postId, string commentText)
        {
            User loggedInUser = (User)Session["LoggedInUser"];
            if (postId >= 0 && commentText != string.Empty)
            {
                Debug.WriteLine("Primljen id posta: " + postId);
                Debug.WriteLine("Primljen komentar: " + commentText);
                if (loggedInUser != null)
                {
                    Comment comment = new Comment(commentText, postId, loggedInUser.Email);
                    CommentData commentData = new CommentData(commentText, postId, loggedInUser.Email);

                    if (comment != null)
                    {
                        // Connect to the comment service
                        ServiceConnector<ICommentService> serviceConnector = new ServiceConnector<ICommentService>();
                        serviceConnector.Connect("net.tcp://localhost:10102/CommentService");
                        ICommentService commentService = serviceConnector.GetProxy();

                        commentService.AddComment(commentData);

                        Post post = AppContext.homePagePostLists.AllPosts.FirstOrDefault(p => p.Id == postId);
                        if (post != null)
                        {
                            Comment commentToAppContext = new Comment
                            {
                                Text = commentData.Text,
                                UserEmail = commentData.UserEmail,
                                PostId = commentData.PostId
                            };
                            post.Comments.Add(commentToAppContext);
                        }
                    }
                }
            }
            return Json(new { success = true, userEmail = loggedInUser.Email });
        }
    }
}