using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace CinemaReservationSystem.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("philonaeem3@gmail.com", "luep zncp tmwg pjzu\r\n")
                };

                return client.SendMailAsync(
                    new MailMessage(from: "philonaeem3@gmail.com",
                                    to: email,
                                    subject,
                                    htmlMessage
                                    )
                    {
                        IsBodyHtml = true
                    }
                    );
            }
        }
    }
}
