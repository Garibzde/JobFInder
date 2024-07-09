
using JobFind.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace JobFind.Service
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMailAsync(string emailto, string subject, string body, bool isHtml = false)
        {
            SmtpClient smtp = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(_configuration["Email:LoginEmail"], _configuration["Email:Pasword"]);
            MailAddress from = new MailAddress(_configuration["Email:LoginEmail"], "JobFind");
            MailAddress to = new MailAddress(emailto);
            MailMessage message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            
            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Email gönderimi sırasında hata oluştu: {ex.Message}");
            }
        }


    }
}
