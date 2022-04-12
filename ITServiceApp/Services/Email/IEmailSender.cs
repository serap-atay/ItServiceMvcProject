using System.Threading.Tasks;
using ITServiceApp.Models;

namespace ITServiceApp.Models.Services
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message); 
    }
}
