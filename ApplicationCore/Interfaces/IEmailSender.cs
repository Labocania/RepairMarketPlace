using System.Threading.Tasks;

namespace RepairMarketPlace.ApplicationCore.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
