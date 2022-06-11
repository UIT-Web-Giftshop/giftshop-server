using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
    public class MailRequestModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }

    public class MailTemplateRequestMode
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public string TemplateName { get; set; }
        public object Model { get; set; }

        public MailTemplateRequestMode(string to, string subject, List<IFormFile> attachments, string templateName, object model)
        {
            To = to;
            Subject = subject;
            Attachments = attachments;
            TemplateName = templateName;
            Model = model;
        }
    }
}