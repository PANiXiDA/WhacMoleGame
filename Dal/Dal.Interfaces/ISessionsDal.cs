using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces.Core;

namespace Dal.Interfaces
{
    public interface ISessionsDal : IBaseDal<DefaultDbContext, Session, Entities.Session, int, SessionsSearchParams, object>
    {
    }
}
