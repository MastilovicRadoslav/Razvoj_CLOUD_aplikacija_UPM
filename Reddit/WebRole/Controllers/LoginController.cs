using Common.Entities;
using Common.Interfaces;
using Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using WebRole.UniversalConnector;

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

			// Connect to the user service
			ServiceConnector<IUserService> serviceConnector = new ServiceConnector<IUserService>();
			serviceConnector.Connect("net.tcp://localhost:10100/UserService");
			IUserService userService = serviceConnector.GetProxy();

			User loggedIn = null;
			List<UserData> users = new List<UserData>();
			if (email != string.Empty && password != string.Empty)
			{
				users = userService.GetAllUsers();
			}

			UserData user = users.Find(u => u.Email == email && u.Password == password);
			loggedIn = new User(user.FirstName, user.LastName, user.Address, user.City, user.Country, user.PhoneNumber, user.Email, user.Password, user.Image);
			if (loggedIn != null)
			{
				Session["LoggedInUser"] = loggedIn;

				// Ispis primljenih podataka od klijenta
				Debug.WriteLine("Email adresa: " + email);
				Debug.WriteLine("Lozina: " + password);
				// Ispis statusa prijave
				Debug.WriteLine("Uspešna prijava!");
				return RedirectToAction("Index", "Home");
			}
			else
			{
				// Ispis primljenih podataka od klijenta
				Debug.WriteLine("Email: " + email);
				Debug.WriteLine("Lozina: " + password);
				// Ispis statusa prijave
				Debug.WriteLine("Prijava je neuspešna!");
				return RedirectToAction("Index", "Home");
			}
		}
	}
}