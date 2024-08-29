using Common.SearchParams;
using Dal.DbModels.Models;
using Dal.DbModels;
using Dal.Interfaces.Core;

namespace Dal.Interfaces
{
    public interface IUsersDal : IBaseDal<DefaultDbContext, User, Entities.User, int, UsersSearchParams, object>
    {
        Task<bool> ExistsAsync(string login);
        Task<Entities.User?> GetAsync(string login);
    }
}
