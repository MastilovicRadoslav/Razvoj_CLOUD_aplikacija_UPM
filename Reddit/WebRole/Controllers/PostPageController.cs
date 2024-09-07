using Common.Comment_queue;
using Common.Entities;
using Common.Interfaces;
using Microsoft.WindowsAzure.Storage.Queue;
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
                    Comment comment = new Comment(commentText, postId, loggedInUser.Email, 0, 0);
                    CommentData commentData = new CommentData(commentText, postId, loggedInUser.Email, comment.Like, comment.Unlike);

                    if (comment != null)
                    {
                        // Connect to the comment service
                        ServiceConnector<ICommentService> serviceConnector = new ServiceConnector<ICommentService>();
                        serviceConnector.Connect("net.tcp://localhost:10102/CommentService");
                        ICommentService commentService = serviceConnector.GetProxy();

                        commentService.AddComment(commentData);
                        CloudQueue queue = QueueHelper.GetQueueReference("comments");
                        queue.AddMessage(new CloudQueueMessage(commentData.ToString()), null, System.TimeSpan.FromMilliseconds(30));

                        Post post = AppContext.homePagePostLists.AllPosts.FirstOrDefault(p => p.Id == postId);
                        if (post != null)
                        {
                            Comment commentToAppContext = new Comment
                            {
                                Text = commentData.Text,
                                UserEmail = commentData.UserEmail,
                                PostId = commentData.PostId,
                                Like = commentData.Like,
                                Unlike = commentData.Unlike
                            };
                            post.Comments.Add(commentToAppContext);
                        }
                    }
                }
            }
            return Json(new { success = true, userEmail = loggedInUser.Email });
        }

        // POST: PostPage/LikeComment
        [HttpPost]
        public ActionResult LikeComment(int commentId)
        {
            ServiceConnector<ICommentService> serviceConnector = new ServiceConnector<ICommentService>();
            serviceConnector.Connect("net.tcp://localhost:10102/CommentService");
            ICommentService commentService = serviceConnector.GetProxy();
            CommentData commentData = commentService.GetComment(commentId);
            commentData.Like++;
            commentService.UpdateComment(commentData); // Promijenjeno: Korištenje metode UpdateComment
            return Json(new { success = true, newLikeCount = commentData.Like });
        }

        // POST: PostPage/UnlikeComment
        [HttpPost]
        public ActionResult UnlikeComment(int commentId)
        {
            ServiceConnector<ICommentService> serviceConnector = new ServiceConnector<ICommentService>();
            serviceConnector.Connect("net.tcp://localhost:10102/CommentService");
            ICommentService commentService = serviceConnector.GetProxy();
            CommentData commentData = commentService.GetComment(commentId);
            commentData.Unlike++;
            commentService.UpdateComment(commentData); // Promijenjeno: Korištenje metode UpdateComment
            return Json(new { success = true, newUnlikeCount = commentData.Unlike });
        }

        // POST: PostPage/LikePost
        [HttpPost]
        public ActionResult LikePost(int postId)
        {
            ServiceConnector<IPostService> serviceConnector = new ServiceConnector<IPostService>();
            serviceConnector.Connect("net.tcp://localhost:10101/PostService");
            IPostService postService = serviceConnector.GetProxy();
            PostData postData = postService.GetPost(postId);
            postData.Like++;
            postService.UpdatePost(postData); // Promijenjeno: Korištenje metode UpdatePost
            return Json(new { success = true, newLikeCount = postData.Like });
        }

        // POST: PostPage/UnlikePost
        [HttpPost]
        public ActionResult UnlikePost(int postId)
        {
            ServiceConnector<IPostService> serviceConnector = new ServiceConnector<IPostService>();
            serviceConnector.Connect("net.tcp://localhost:10101/PostService");
            IPostService postService = serviceConnector.GetProxy();
            PostData postData = postService.GetPost(postId);
            postData.UnLike++;
            postService.UpdatePost(postData); // Promijenjeno: Korištenje metode UpdatePost
            return Json(new { success = true, newUnlikeCount = postData.UnLike });
        }


    }
}