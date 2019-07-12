using Microsoft.Extensions.Options;
using VDS.UMS.Interfaces;
using VDS.UMS.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace VDS.UMS.Services
{
    public class EmailService : IEmailService
    {
        private EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient())
            {
                try
                {


                    var credential = new NetworkCredential
                    {
                        UserName = _settings.Email,
                        Password = _settings.Password
                    };

                    client.Credentials = credential;
                    client.Host = _settings.Host;
                    client.Port = int.Parse(_settings.Port);
                    client.EnableSsl = true;

                    using (var emailMessage = new MailMessage())
                    {
                        emailMessage.To.Add(new MailAddress(email));
                        emailMessage.From = new MailAddress(_settings.Email);
                        emailMessage.Subject = subject;
                        emailMessage.Body = message;
                        client.Send(emailMessage);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            // TODO: Wire this up to actual email sending logic via SendGrid, local SMTP, etc.
            return Task.CompletedTask;
        }
    }
}
