using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string smtpServer = "smtp.gmail.com";         
    private readonly int smtpPort = 587;                           
    private readonly string smtpUser = "andresvertel2021@gmail.com"; 
    private readonly string smtpPass = "xxcushjinjrbwjor";    

    public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(smtpUser));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = htmlMessage };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            
            await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);

            
            await smtp.AuthenticateAsync(smtpUser, smtpPass);

            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al enviar el correo: {ex.Message}");
            throw;
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}
