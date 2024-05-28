using Common.Entities;
using Common.Interfaces;
using Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using WebRole.Blob;
using WebRole.ImagePathConverter;
using WebRole.UniversalConnector;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostPathConverter pathConverter = new PostPathConverter();
        private readonly BlobHelper blobHelper = new BlobHelper();

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

                // Connect to the comment service
                ServiceConnector<ICommentService> serviceConnectorComment = new ServiceConnector<ICommentService>();
                serviceConnectorComment.Connect("net.tcp://localhost:10102/CommentService");
                ICommentService commentService = serviceConnectorComment.GetProxy();

                List<PostData> allPostsFromTable = postService.GetAllPosts();
                List<CommentData> allCommentsFromTable = commentService.GetAllComments();
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

                    foreach (var commentData in allCommentsFromTable)
                    {
                        if (commentData.PostId == postData.Id)
                        {
                            Comment comment = new Comment
                            {
                                Text = commentData.Text,
                                UserEmail = commentData.UserEmail,
                                PostId = commentData.PostId
                            };
                            post.Comments.Add(comment);
                        }
                    }

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
                newPost.Image = pathConverter.ReplacePath(postImage);
                newPost.UserEmail = loggedInUser.Email;
                newPost.Like = 0;
                newPost.UnLike = 0;
                postdata = new PostData(newPost.Title, newPost.Description, newPost.Image, newPost.UserEmail, newPost.Like, newPost.UnLike);

                if (postImage != string.Empty)
                {
                    string imagePath = pathConverter.ReplacePath(postImage);
                    string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                    string pathToImageInProject = projectDirectory + imagePath;
                    if (!System.IO.File.Exists(pathToImageInProject))
                    {
                        Debug.WriteLine("Image does not exist at the specified path: " + imagePath);
                    }
                    else
                    {
                        using (Image imageToBlob = Image.FromFile(pathToImageInProject))
                        {
                            string containerName = "redditpostimages";
                            string blobName = string.Format("image_{0}", newPost.Id);
                            string imageUrl = blobHelper.UploadImage(imageToBlob, containerName, blobName);
                            Debug.WriteLine("Image uploaded to: " + imageUrl);
                        }
                    }
                }

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

        // POST: Home/AddToFavorites
        // Obrada dodavanja posta u favorite
        [HttpPost]
        public ActionResult AddToFavorites(int postId)
        {
            if (postId >= 0)
            {
                Debug.WriteLine("Ispis IDa posta: " + postId);

                // Connect to the post service
                ServiceConnector<IFavouritesService> serviceConnector = new ServiceConnector<IFavouritesService>();
                serviceConnector.Connect("net.tcp://localhost:10103/FavouritesService");
                IFavouritesService favouritesService = serviceConnector.GetProxy();

                User loggedInUser = (User)Session["LoggedInUser"];
                if (loggedInUser != null)
                {
                    Favourites newFav = new Favourites(postId, loggedInUser.Email);
                    favouritesService.AddToFavourites(newFav);
                }
            }
            return RedirectToAction("Home", "Index");
        }
    }
}