using Common.Entities;
using Common.Interfaces;
using Models;
using System.Diagnostics;
using System.Web.Mvc;
using WebRole.ImagePathConverter;
using WebRole.UniversalConnector;

namespace WebRole.Controllers
{
	public class RegistrationController : Controller
	{
		private readonly PathConverter pathConverter = new PathConverter();

		// GET: Registration
		// Prikaz registracije
		public ActionResult Registration()
		{
			return View();
		}

		// POST: Registration
		// Obrada registracije
		[HttpPost]
		public ActionResult SubmitRegistration()
		{
			var fname = Request["fname"] ?? string.Empty;
			var lname = Request["lname"] ?? string.Empty;
			var adress = Request["adress"] ?? string.Empty;
			var city = Request["city"] ?? string.Empty;
			var state = Request["country"] ?? string.Empty;
			var phoneNumber = Request["phoneNumber"] ?? string.Empty;
			var email = Request["email"] ?? string.Empty;
			var password = Request["password"] ?? string.Empty;
			var image = Request["image"] ?? string.Empty;

			User u = null;
			UserData newUser = null;
			if (fname != string.Empty && lname != string.Empty && adress != string.Empty && city != string.Empty && state != string.Empty && phoneNumber != string.Empty && email != string.Empty && password != string.Empty)
			{
				string imagePath = pathConverter.ReplacePath(image);
				u = new User(fname, lname, adress, city, state, phoneNumber, email, password, imagePath);
				newUser = new UserData(fname, lname, adress, city, state, phoneNumber, email, password, imagePath);
			}

			// Connect to the user service
			ServiceConnector<IUserService> serviceConnector = new ServiceConnector<IUserService>();
			serviceConnector.Connect("net.tcp://localhost:10100/UserService");
			IUserService userService = serviceConnector.GetProxy();

			if (u != null)
			{
				// Ispis primljenih podataka od klijenta
				Debug.WriteLine("Ime: " + u.FirstName);
				Debug.WriteLine("Prezime: " + u.LastName);
				Debug.WriteLine("Adresa: " + u.Address);
				Debug.WriteLine("Grad: " + u.City);
				Debug.WriteLine("Država: " + u.Country);
				Debug.WriteLine("Broj telefona: " + u.PhoneNumber);
				Debug.WriteLine("Email: " + u.Email);
				Debug.WriteLine("Lozinka: " + u.Password);
				Debug.WriteLine("Slika: " + u.Image);

				userService.AddUser(newUser);
				// Ispis statusa registracije
				Debug.WriteLine("Uspešno dodat u tabelu!");
				Debug.WriteLine("Uspešna registracija!");
				return RedirectToAction("Login", "Login");
			}
			else
			{
				// Ispis primljenih podataka od klijenta
				Debug.WriteLine("Ime: " + u.FirstName);
				Debug.WriteLine("Prezime: " + u.LastName);
				Debug.WriteLine("Adresa: " + u.Address);
				Debug.WriteLine("Grad: " + u.City);
				Debug.WriteLine("Država: " + u.Country);
				Debug.WriteLine("Broj telefona: " + u.PhoneNumber);
				Debug.WriteLine("Email: " + u.Email);
				Debug.WriteLine("Lozinka: " + u.Password);
				Debug.WriteLine("Slika: " + u.Image);
				// Ispis statusa registracije
				Debug.WriteLine("Registracija nije uspela!");
				return RedirectToAction("Registration", "Registration");
			}
		}
	}
}