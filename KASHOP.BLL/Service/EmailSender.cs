using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("randethar@gmail.com", "hmho jhzt kckr wmrn")
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("randethar@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}