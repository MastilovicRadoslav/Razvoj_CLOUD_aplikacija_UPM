using AdminTools.AdminManagement;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;

namespace AdminTools
{
    public class Program
    {
        public static List<string> EmailAddresses = new List<string>();

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

                    ManageEmailAddresses();

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

        private static void ManageEmailAddresses()
        {
            string command = "";
            while (command != "exit")
            {
                Console.WriteLine("1. Unesite 'add' za dodavanje email adrese");
                Console.WriteLine("2. Unesite 'remove' za uklanjanje email adrese");
                Console.WriteLine("3. Unesite 'update' za ažuriranje email adrese");
                Console.WriteLine("4. Unesite 'list' za prikaz svih email adresa");
                Console.WriteLine("5. Unesite 'exit' za izlaz:");
                Console.Write("Unos: ");
                command = Console.ReadLine().ToLower();

                switch (command)
                {
                    case "add":
                        Console.Write("Unesite email adresu za dodavanje: ");
                        string emailToAdd = Console.ReadLine();
                        if (!EmailAddresses.Contains(emailToAdd))
                        {
                            EmailAddresses.Add(emailToAdd);
                            Console.WriteLine($"Email adresa {emailToAdd} je dodata.");
                        }
                        else
                        {
                            Console.WriteLine($"Email adresa {emailToAdd} već postoji.");
                        }
                        break;

                    case "remove":
                        Console.Write("Unesite email adresu za uklanjanje: ");
                        string emailToRemove = Console.ReadLine();
                        if (EmailAddresses.Contains(emailToRemove))
                        {
                            EmailAddresses.Remove(emailToRemove);
                            Console.WriteLine($"Email adresa {emailToRemove} je uklonjena.");
                        }
                        else
                        {
                            Console.WriteLine($"Email adresa {emailToRemove} nije pronađena.");
                        }
                        break;

                    case "update":
                        Console.Write("Unesite email adresu koju želite da ažurirate: ");
                        string emailToUpdate = Console.ReadLine();
                        if (EmailAddresses.Contains(emailToUpdate))
                        {
                            Console.Write("Unesite novu email adresu: ");
                            string newEmail = Console.ReadLine();
                            int index = EmailAddresses.IndexOf(emailToUpdate);
                            EmailAddresses[index] = newEmail;
                            Console.WriteLine($"Email adresa {emailToUpdate} je ažurirana na {newEmail}.");
                        }
                        else
                        {
                            Console.WriteLine($"Email adresa {emailToUpdate} nije pronađena.");
                        }
                        break;

                    case "list":
                        Console.WriteLine("Trenutne email adrese:");
                        foreach (var email in EmailAddresses)
                        {
                            Console.WriteLine(email);
                        }
                        break;

                    case "exit":
                        Console.WriteLine("Izlazak iz menadžera email adresa.");
                        break;

                    default:
                        Console.WriteLine("Nepoznata komanda.");
                        break;
                }
            }
        }
    }

    public class Startup //Startup pokrece AdminHub na onom gore definisanom portu
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
