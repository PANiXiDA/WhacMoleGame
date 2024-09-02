using Common.SearchParams;
using Entities;

namespace BL.Interfaces
{
    public interface ISessionsBL : ICrudBL<Session, SessionsSearchParams, object>
    {
        Task<List<int>> AddOrUpdateAsync(IList<Session> entity);
    }
}
