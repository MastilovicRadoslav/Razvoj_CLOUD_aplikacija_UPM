using Common.Entities;
using Common.Interfaces;
using Models;
using System.Diagnostics;
using System.Web.Mvc;
using WebRole.UniversalConnector;

namespace WebRole.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        // Prikaz profila
        public ActionResult ShowProfile()
        {
            User loggedInUser = (User)Session["LoggedInUser"];
            if (loggedInUser != null)
            {
                return View(loggedInUser);
            }
            return View();
        }

        // POST: Profile
        // Obrada izmene profila
        [HttpPost]
        public ActionResult EditProfile()
        {
            string fName = Request["fname"] ?? string.Empty;
            string lName = Request["lname"] ?? string.Empty;
            string address = Request["address"] ?? string.Empty;
            string city = Request["city"] ?? string.Empty;
            string country = Request["country"] ?? string.Empty;
            string phoneNumber = Request["phonenumber"] ?? string.Empty;
            string email = Request["email"] ?? string.Empty;
            string password = Request["password"] ?? string.Empty;
            string image = Request["image"] ?? string.Empty;
            string imagepath = "/Images/Profile/" + image;

            // Connect to the user service
            ServiceConnector<IUserService> serviceConnector = new ServiceConnector<IUserService>();
            serviceConnector.Connect("net.tcp://localhost:10100/UserService");
            IUserService userService = serviceConnector.GetProxy();

            User updateUser = null;
            UserData updateUserData = null;
            if (fName != string.Empty && lName != string.Empty && address != string.Empty && city != string.Empty && country != string.Empty && phoneNumber != string.Empty && email != string.Empty && password != string.Empty)
            {
                updateUser = new User(fName, lName, address, city, country, phoneNumber, email, password, imagepath);
                updateUserData = new UserData(fName, lName, address, city, country, phoneNumber, email, password, imagepath);
                Session["LoggedInUser"] = updateUser;

                // Ispis primljenih podataka od klijenta
                Debug.WriteLine("Ime: " + updateUser.FirstName);
                Debug.WriteLine("Prezime: " + updateUser.LastName);
                Debug.WriteLine("Adresa: " + updateUser.Address);
                Debug.WriteLine("Grad: " + updateUser.City);
                Debug.WriteLine("Država: " + updateUser.Country);
                Debug.WriteLine("Broj telefona: " + updateUser.PhoneNumber);
                Debug.WriteLine("Email: " + updateUser.Email);
                Debug.WriteLine("Lozinka: " + updateUser.Password);
                Debug.WriteLine("Slika: " + updateUser.Image);

                userService.UpdateUser(updateUser.Email, updateUserData);
                // Ispis statusa ažuriranja profila
                Debug.WriteLine("Profil je uspešno ažuriran!");
                return RedirectToAction("ShowProfile", "Profile");
            }
            else
            {
                // Ispis primljenih podataka od klijenta
                Debug.WriteLine("Ime: " + updateUser.FirstName);
                Debug.WriteLine("Prezime: " + updateUser.LastName);
                Debug.WriteLine("Adresa: " + updateUser.Address);
                Debug.WriteLine("Grad: " + updateUser.City);
                Debug.WriteLine("Država: " + updateUser.Country);
                Debug.WriteLine("Broj telefona: " + updateUser.PhoneNumber);
                Debug.WriteLine("Email: " + updateUser.Email);
                Debug.WriteLine("Lozinka: " + updateUser.Password);
                Debug.WriteLine("Slika: " + updateUser.Image);
                // Ispis statusa ažuriranja profila
                Debug.WriteLine("Ažuriranje profila je neuspešno!");
                return RedirectToAction("ShowProfile", "Profile");
            }
        }
    }
}