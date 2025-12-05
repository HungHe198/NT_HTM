using System.Threading.Tasks;

namespace NT.WEB.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlBody);
    }
}