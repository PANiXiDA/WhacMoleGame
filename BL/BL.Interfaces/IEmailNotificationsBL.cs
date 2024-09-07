using Entities;

namespace BL.Interfaces
{
    public interface IEmailNotificationsBL
    {
        Task SendRecoveryPasswordEmailAsync(string toEmail, User user);
    }
}
