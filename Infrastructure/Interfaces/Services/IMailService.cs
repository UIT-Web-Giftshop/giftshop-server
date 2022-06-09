using System.Threading.Tasks;
using Domain.Models;

namespace Infrastructure.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequestModel request);

        Task SendWithTemplate(string templateName, string to, object[] args);
    }
}