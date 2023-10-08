using Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.MailHelper
{
    public class EmailHelper : IEmailHelper
    {
        public bool SendEmail(string mailAddress, string token, bool bodyHtml)
        {
            var emailAddress = EmailConfiguration.Email;
            var emailPassword = EmailConfiguration.Password;
            string senderEmail = emailAddress;
            string senderPassword = emailPassword;

            //Create the SMTP client
            SmtpClient smtpClient = new(EmailConfiguration.Smtp, EmailConfiguration.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            try
            {
                // Create the email message
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(senderEmail)
                };
                mailMessage.To.Add(mailAddress);
                mailMessage.Subject = $"Message from - {EmailConfiguration.Email}";
                mailMessage.Body = $"<a href='https://localhost:7042/api/auth/verifypassword?email={mailAddress}&token={token}'>Tesdiq et</a>";
                // Specify that the body contains HTML
                mailMessage.IsBodyHtml = true;
                // Send the email
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}
