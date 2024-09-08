using Entities;

namespace BL.Interfaces
{
    public interface IEmailNotificationsBL
    {
        Task SendRecoveryPasswordEmailAsync(string toEmail, User user);
        Task SendCodeConfirmation(string toEmail, int code);
    }
}
