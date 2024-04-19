using System.Net.Mail;
using System.Net;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service.Implementation
{
    public class EmailService:IEmailService
    {
        //private readonly string _smtpServer;
        //private readonly int _port;
        //private readonly string _username;
        //private readonly string _password;

        //public EmailService(string smtpServer, int port, string username, string password)
        //{
        //    _smtpServer = smtpServer;
        //    _port = port;
        //    _username = username;
        //    _password = password;
        //}

        public async Task SendEmailAsync(Email message)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("bhagyashree.gaikwad@wonderbiz.in", "tpli pogm axqq egvk");
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body)
                    {
                        IsBodyHtml = true
                    };
                    if (!string.IsNullOrEmpty(message.CC))
                    {
                        mailMessage.CC.Add(message.CC);
                    }

                    await smtpClient.SendMailAsync(mailMessage);
                 
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log or rethrow)
                throw;
            }
        }
    }
}
