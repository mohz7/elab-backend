using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace eLab.PL.Helper
{
    public class EmailSetting : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("elabsystem1@gmail.com", "ivjt wqux ycqu ojss")
                };

                await client.SendMailAsync(
                    new MailMessage(
                        from: "elabsystem1@gmail.com",
                        to: email,
                        subject,
                        htmlMessage
                    )
                    { IsBodyHtml = true }
                );

                Console.WriteLine("✅ Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"Details: {ex.InnerException?.Message}");
            }
        }
    }
    
}

