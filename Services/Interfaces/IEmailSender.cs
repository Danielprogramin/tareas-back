using System.Net;
using System.Net.Mail;
using TareasApi.configuration;

namespace TareasApi.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

}
