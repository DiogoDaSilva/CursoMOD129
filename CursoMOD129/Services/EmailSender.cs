using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace CursoMOD129.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Credentials = new NetworkCredential("cursoMOD129@gmail.com", "vlxcfjtulmrvnyle"),
                Port = 587,
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("\"Clínica Médica Dentária XPTO\" <cursoMOD129@gmail.com>"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };


            mailMessage.To.Add(email);

            mailMessage.Bcc.Add("cursoMOD129@gmail.com");

            smtpClient.Send(mailMessage);

            return Task.CompletedTask;
        }
    }
}
