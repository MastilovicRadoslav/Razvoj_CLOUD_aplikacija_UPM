using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Notification_via_email
{
    public class EmailSender
    {
        public void SendEmail(string recipient, string subject, string body)
        {
            string fromMail = "drsprojekat2023@gmail.com";
            string fromPassword = "xrnu nktr zprh vvqk";
            string toMail = recipient;

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
                Debug.WriteLine("Email je uspešno poslat!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Došlo je do greške tokom slanja emaila: {ex.Message}");
            }
        }
    }
}