using Common.Entities;
using Common.Interfaces;
using Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using WebRole.UniversalConnector;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home page
        // Prikaz početne stranice
        public ActionResult Index()
        {
            User loggedInUser = (User)Session["LoggedInUser"];
            if (loggedInUser == null)
            {
                ViewBag.AlertMessage = "Niste prijavljeni!";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                // Connect to the post service
                ServiceConnector<IPostService> serviceConnector = new ServiceConnector<IPostService>();
                serviceConnector.Connect("net.tcp://localhost:10101/PostService");
                IPostService postService = serviceConnector.GetProxy();

                List<PostData> allPostsFromTable = postService.GetAllPosts();
                List<Post> allPosts = new List<Post>();
                List<Post> myPosts = new List<Post>();
                foreach (var postData in allPostsFromTable)
                {
                    Post post = new Post
                    {
                        Id = postData.Id,
                        Title = postData.Title,
                        Description = postData.Description,
                        Image = postData.Image,
                        Comments = new List<Comment>(),
                        UserEmail = postData.UserEmail,
                        Like = postData.Like,
                        UnLike = postData.UnLike
                    };
                    allPosts.Add(post);

                    if (post.UserEmail.Equals(loggedInUser.Email))
                    {
                        myPosts.Add(post);
                    }
                }

                AppContext.homePagePostLists.AllPosts = allPosts;
                AppContext.homePagePostLists.MyPosts = myPosts;
                return View(AppContext.homePagePostLists);
            }
        }

        // POST: Home/OpenPost
        // Obrada zahteva za otvaranje stranice sa nekom temom
        [HttpPost]
        public ActionResult OpenPost(int postId)
        {
            Debug.WriteLine("Primljen id posta: " + postId);
            return Json(new { success = true, postId = postId });
        }

        // POST: Add new post
        // Obrada postavljanje novog posta
        [HttpPost]
        public ActionResult AddNewPost()
        {
            var postTitle = Request["postTitleModal"] ?? string.Empty;
            var postDescription = Request["postDescriptionModal"] ?? string.Empty;
            var postImage = Request["imageModal"] ?? string.Empty;

            // Ispis primljenih podataka od klijenta
            Debug.WriteLine("Naslov: " + postTitle);
            Debug.WriteLine("Opis: " + postDescription);
            Debug.WriteLine("Slika: " + postImage);
            Debug.WriteLine("Došlo je do greške!");

            Post newPost = new Post();
            PostData postdata = null;
            if (postTitle != string.Empty && postDescription != string.Empty)
            {
                User loggedInUser = (User)Session["LoggedInUser"];
                newPost.Title = postTitle;
                newPost.Description = postDescription;
                newPost.Image = postImage;
                newPost.UserEmail = loggedInUser.Email;
                newPost.Like = 0;
                newPost.UnLike = 0;
                postdata = new PostData(newPost.Title, newPost.Description, newPost.Image, newPost.UserEmail, newPost.Like, newPost.UnLike);

                // Connect to the post service
                ServiceConnector<IPostService> serviceConnector = new ServiceConnector<IPostService>();
                serviceConnector.Connect("net.tcp://localhost:10101/PostService");
                IPostService postService = serviceConnector.GetProxy();

                if (newPost != null)
                {
                    postService.AddPost(postdata);
                    Debug.WriteLine("Uspešno ste dodali novi post!");
                    ViewBag.AlertMessage = "Uspešno ste dodali novi post!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Debug.WriteLine("Došlo je do greške!");
                    ViewBag.AlertMessage = "Došlo je do greške!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                Debug.WriteLine("Došlo je do greške!");
                ViewBag.AlertMessage = "Došlo je do greške!";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Home/PostSort
        // Obrada zahteva za sortitanje
        [HttpPost]
        public ActionResult PostSort(string sortOption)
        {
            if (sortOption != string.Empty)
            {
                Debug.WriteLine("Ispis opcije: " + sortOption);
                AppContext.homePagePostLists.AllPosts = AppContext.homePagePostLists.AllPosts.OrderBy(post => post.Title).ToList();
            }
            return Json(AppContext.homePagePostLists.AllPosts);
        }

        // POST: Home/PostSort
        // Obrada zahteva za pretragu po nazivu
        [HttpPost]
        public ActionResult PostSearch(string searchText)
        {
            if (searchText != string.Empty)
            {
                Debug.WriteLine("Primljen tekst: " + searchText);
                var matchingPosts = AppContext.homePagePostLists.AllPosts.Where(post => post.Title.Contains(searchText)).ToList();
                AppContext.homePagePostLists.AllPosts = matchingPosts;
            }
            return Json(AppContext.homePagePostLists.AllPosts);
        }

        // POST: Home/DeletePost
        // Obrada brisanja posta
        [HttpPost]
        public ActionResult DeletePost(int postId)
        {
            if (postId >= 0)
            {
                var postToRemove = AppContext.homePagePostLists.AllPosts.FirstOrDefault(post => post.Id == postId);
                if (postToRemove != null)
                {
                    // Connect to the post service
                    ServiceConnector<IPostService> serviceConnector = new ServiceConnector<IPostService>();
                    serviceConnector.Connect("net.tcp://localhost:10101/PostService");
                    IPostService postService = serviceConnector.GetProxy();

                    bool retV = postService.RemovePost(postId);
                    if (retV)
                    {
                        AppContext.homePagePostLists.AllPosts.Remove(postToRemove);
                        AppContext.homePagePostLists.MyPosts.Remove(postToRemove);
                    }
                }
            }
            return RedirectToAction("Home", "Index");
        }
    }
}