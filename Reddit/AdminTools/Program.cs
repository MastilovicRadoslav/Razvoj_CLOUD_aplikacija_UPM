using AdminTools.AdminManagement;
using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace AdminTools
{
    public class Program
    {
        static void Main()
        {
            bool isAuthenticated = false;
            Administrators administrators = new Administrators();

            Console.WriteLine("*************************************************************************");
            Console.WriteLine("Dobrodošli u administratorsku konzolu. Molimo vas prijavite se!");
            Console.WriteLine("*************************************************************************\n");

            do
            {
                Console.Write("Unesite korisničko ime: ");
                string userName = Console.ReadLine();
                Console.Write("Unesite lozinku: ");
                string password = Console.ReadLine();

                foreach (var admin in administrators.Admins)
                {
                    if (admin.UserName == userName && admin.Password == password)
                    {
                        isAuthenticated = true;
                        break;
                    }
                }

                if (isAuthenticated)
                {
                    Console.WriteLine("\nUspešna prijava!\n");

                    string url = "http://localhost:8080";
                    using (WebApp.Start<Startup>(url))
                    {
                        Console.WriteLine($"Server je dostupan na: {url}\n");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nPogrešili ste korisničko ime ili lozinku! Pokušajte ponovo.\n");
                }
            } while (!isAuthenticated);
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}