using Microsoft.AspNet.SignalR;
using System;
using System.Net;
using System.Net.Mail;

namespace AdminTools.My_SignalR_Hub
{
    public class AdminHub : Hub
    {
        public void SendMessage(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
            HandleIncomingMessage(name, message);
        }

        private void HandleIncomingMessage(string name, string message)
        {
            if (message.Equals("1"))
            {
                Console.WriteLine($"Obaveštava se admin da {name} nije dostupan!\n");
                string subject = "Neki od servisa je pao!";
                string body = $"{name} trenutno nije dostupan!\nMolimo vas da uradite nešto kako bi korisnici ponovo mogli da koriste platformu.";
                SendEmail(subject, body);
            }
        }

        void SendEmail(string subject, string body)
        {
            string fromMail = "drsprojekat2023@gmail.com";
            string fromPassword = "xrnu nktr zprh vvqk";
            string toMail = "drsprojekat2023@gmail.com";

            MailMessage message = new MailMessage
            {
                From = new MailAddress(fromMail),
                Subject = subject
            };
            message.To.Add(new MailAddress(toMail));
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            try
            {
                smtpClient.Send(message);
                Console.WriteLine("Email uspešno poslat!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Došlo je do greške tokom slanja emaila: {ex.Message}");
            }
        }
    }
}