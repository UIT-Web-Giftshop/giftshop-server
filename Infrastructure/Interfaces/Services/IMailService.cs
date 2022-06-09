using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequestModel request);

        Task SendWithTemplate(string to, string subject, List<IFormFile> attachments, string templateName,  dynamic model);
    }
}