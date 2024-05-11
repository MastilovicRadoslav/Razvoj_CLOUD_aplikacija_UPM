using Models;
using System.Diagnostics;
using System.Web.Mvc;

namespace Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        // Prikaz za prijavu
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        // Obrada prijave
        [System.Web.Mvc.HttpPost]
        public ActionResult SubmitLogin()
        {
            var email = Request["email"] ?? string.Empty;
            var password = Request["password"] ?? string.Empty;

            User user = AppContext.Users.Find(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                Session["LoggedInUser"] = user;

                // Ispis primljenih podataka od klijenta
                Debug.WriteLine("Email adresa: " + email);
                Debug.WriteLine("Lozina: " + password);
                Debug.WriteLine("Uspešna prijava!");
                return RedirectToAction("Index", "Home");
            }

            // Ispis primljenih podataka od klijenta
            Debug.WriteLine("Email: " + email);
            Debug.WriteLine("Lozina: " + password);
            Debug.WriteLine("Prijava je neuspešna!");
            return RedirectToAction("Index", "Home");
        }
    }
}