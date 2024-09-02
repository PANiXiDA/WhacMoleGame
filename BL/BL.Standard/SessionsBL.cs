using Common.SearchParams.Core;
using Common.SearchParams;
using Dal.Interfaces;
using BL.Interfaces;
using Entities;

namespace BL.Standard
{
    public class SessionsBL : ISessionsBL
    {
        private readonly ISessionsDal _sessionsDal;

        public SessionsBL(ISessionsDal sessionsDal)
        {
            _sessionsDal = sessionsDal;
        }

        public async Task<int> AddOrUpdateAsync(Session entity)
        {
            entity.Id = await _sessionsDal.AddOrUpdateAsync(entity);
            return entity.Id;
        }

        public async Task<List<int>> AddOrUpdateAsync(IList<Session> entities)
        {
            var ids = await _sessionsDal.AddOrUpdateAsync(entities);
            return ids.ToList();
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _sessionsDal.ExistsAsync(id);
        }

        public Task<bool> ExistsAsync(SessionsSearchParams searchParams)
        {
            return _sessionsDal.ExistsAsync(searchParams);
        }

        public Task<Session> GetAsync(int id, object? convertParams = null)
        {
            return _sessionsDal.GetAsync(id, convertParams);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _sessionsDal.DeleteAsync(id);
        }

        public Task<SearchResult<Session>> GetAsync(SessionsSearchParams searchParams, object? convertParams = null)
        {
            return _sessionsDal.GetAsync(searchParams, convertParams);
        }
    }
}
