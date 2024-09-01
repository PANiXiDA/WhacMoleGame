using Common.ConvertParams;
using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces.Core;

namespace Dal.Interfaces
{
    public interface IUsersDal : IBaseDal<DefaultDbContext, User, Entities.User, int, UsersSearchParams, UsersConvertParams>
    {
        Task<bool> ExistsAsync(string login);
        Task<Entities.User?> GetAsync(string login);
    }
}
