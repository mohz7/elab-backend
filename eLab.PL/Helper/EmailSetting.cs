using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace eLab.PL.Helper
{
    public class EmailSetting : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("abood7tt@gmail.com", "cyff xpyw ymbx vabt")
            };

            return client.SendMailAsync(
                new MailMessage(from: "abood7tt@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                { IsBodyHtml = true});
        }
    }
    
}
