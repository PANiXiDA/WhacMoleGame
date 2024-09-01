using Common.ConvertParams;
using Common.SearchParams;
using Entities;

namespace BL.Interfaces
{
    public interface IUsersBL : ICrudBL<User, UsersSearchParams, UsersConvertParams>
    {
        Task<bool> ExistAsync(string login);
        Task<User?> GetAsync(string login);
        Task<User?> VerifyPasswordAsync(string login, string password);
    }
}
