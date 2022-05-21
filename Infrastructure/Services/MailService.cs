using System.IO;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Settings;
using Infrastructure.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(MailRequestModel request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;

            var builder = new BodyBuilder();
            if (request.Attachments is not null)
            {
                byte[] fileBytes;
                foreach (var file in request.Attachments)
                {
                    if (file.Length <= 0) continue;
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    builder.Attachments.Add(file.Name, fileBytes, ContentType.Parse(file.ContentType));
                }
            }

            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
            
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendWithTemplate(string templateName, string to, object[] args)
        {
            // TODO: use template from database
            const string htmlRaw = "Hello {0}, this is test template email from {1}";
            var html = string.Format(htmlRaw, args);
            var request = new MailRequestModel
            {
                Body = html,
                Subject = templateName,
                To = to
            };
            
            await SendAsync(request);
        }
    }
}