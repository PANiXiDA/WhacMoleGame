using Common.SearchParams.Core;
using Common.SearchParams;
using Dal.Interfaces;
using BL.Interfaces;
using Entities;
using Common.ConvertParams;

namespace BL.Standard
{
    public class GamesBL : IGamesBL
    {
        private readonly IGamesDal _gamesDal;

        public GamesBL(IGamesDal gamesDal)
        {
            _gamesDal = gamesDal;
        }

        public async Task<int> AddOrUpdateAsync(Game entity)
        {
            entity.Id = await _gamesDal.AddOrUpdateAsync(entity);
            return entity.Id;
        }

        public async Task<List<int>> AddOrUpdateAsync(IList<Game> entities)
        {
            var ids = await _gamesDal.AddOrUpdateAsync(entities);
            return ids.ToList();
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _gamesDal.ExistsAsync(id);
        }

        public Task<bool> ExistsAsync(GamesSearchParams searchParams)
        {
            return _gamesDal.ExistsAsync(searchParams);
        }

        public Task<Game> GetAsync(int id, GamesConvertParams? convertParams = null)
        {
            return _gamesDal.GetAsync(id, convertParams);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _gamesDal.DeleteAsync(id);
        }

        public Task<SearchResult<Game>> GetAsync(GamesSearchParams searchParams, GamesConvertParams? convertParams = null)
        {
            return _gamesDal.GetAsync(searchParams, convertParams);
        }
    }
}
