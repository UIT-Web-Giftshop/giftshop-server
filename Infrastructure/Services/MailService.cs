using System.IO;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Settings;
using Infrastructure.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorEmailLibs.Constants;
using RazorEmailLibs.Services;
using RazorEmailLibs.Views.Emails.ConfirmAccount;

namespace Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public MailService(IOptions<MailSettings> mailSettings, IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _razorViewToStringRenderer = razorViewToStringRenderer;
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
            var confirmEmailViewModel = new ConfirmAccountEmailViewModel("http://localhost/confirm");

            var html = await _razorViewToStringRenderer.RenderViewToStringAsync(
                MailTemplatesName.CONFIRM_ACCOUNT_EMAIL,
                confirmEmailViewModel);

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