using Common.SearchParams;
using Entities;

namespace BL.Interfaces
{
    public interface IUsersBL : ICrudBL<User, UsersSearchParams, object>
    {
        Task<bool> ExistAsync(string login);
        Task<User?> GetAsync(string login);
        Task<User?> VerifyPasswordAsync(string login, string password);
    }
}
