using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.ServiceInterfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Services
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration configuration) { 
        
            _config = configuration;
        }

        public void SendEmail(EmailDto emailDto)
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("Hairlytics", from));
            emailMessage.To.Add(new MailboxAddress(emailDto.To, emailDto.To));
            emailMessage.Subject = emailDto.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailDto.Body)
            };


            using (var client = new SmtpClient()) {
                try
                {
                    client.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex) {

                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            
            }
           

        }
    }
}
