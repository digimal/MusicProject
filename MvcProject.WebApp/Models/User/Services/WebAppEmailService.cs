using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MvcProject.WebApp.Models.User.Services
{
    public class WebAppEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string from = ((SmtpSection) ConfigurationManager.GetSection("system.net/mailSettings/smtp")).From; 
            var mail = new MailMessage(from, message.Destination)
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
            return new SmtpClient().SendMailAsync(mail);
        }
    }
}