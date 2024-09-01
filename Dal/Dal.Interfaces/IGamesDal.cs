using Common.ConvertParams;
using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces.Core;

namespace Dal.Interfaces
{
    public interface IGamesDal : IBaseDal<DefaultDbContext, Game, Entities.Game, int, GamesSearchParams, GamesConvertParams>
    {
    }
}
