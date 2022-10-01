using System.Threading.Tasks;

namespace InvoicePro.Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string content, string email);
    }
}