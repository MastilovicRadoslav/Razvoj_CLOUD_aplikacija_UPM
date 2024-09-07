using Common.Cryptography;
using Common.Entities;
using Common.Interfaces;
using Models;
using System.Diagnostics;
using System.Web.Mvc;
using WebRole.UniversalConnector;

namespace Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult SubmitLogin()
        {
            var email = Request["email"] ?? string.Empty;
            var password = Request["password"] ?? string.Empty;

            // Connect to the user service
            ServiceConnector<IUserService> serviceConnector = new ServiceConnector<IUserService>();
            serviceConnector.Connect("net.tcp://localhost:10100/UserService");
            IUserService userService = serviceConnector.GetProxy();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                UserData user = userService.GetUser(email);

                if (user != null && PasswordHasher.VerifyPassword(password, user.Password))
                {
                    User loggedIn = new User(user.FirstName, user.LastName, user.Address, user.City, user.Country, user.PhoneNumber, user.Email, user.Password, user.Image);
                    Session["LoggedInUser"] = loggedIn;

                    // Logging received client data
                    Debug.WriteLine("Email adresa: " + email);
                    Debug.WriteLine("Lozinka: " + password);
                    // Logging login status
                    Debug.WriteLine("Uspešna prijava!");
                    return RedirectToAction("Index", "Home");
                }
            }

            // Logging received client data
            Debug.WriteLine("Email: " + email);
            Debug.WriteLine("Lozinka: " + password);
            // Logging login status
            Debug.WriteLine("Prijava je neuspešna!");
            return RedirectToAction("Index", "Home");
        }
    }
}
