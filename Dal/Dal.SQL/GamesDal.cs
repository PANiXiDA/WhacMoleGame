using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Common.ConvertParams;
using Dal.Interfaces;

namespace Dal.SQL
{
    public class GamesDal : BaseDal<DefaultDbContext, Game, Entities.Game, int, GamesSearchParams, GamesConvertParams>, IGamesDal
    {
        protected override bool RequiresUpdatesAfterObjectSaving => false;

        public GamesDal(DefaultDbContext context) : base(context) { }

        protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Game entity, Game dbObject, bool exists)
        {
            dbObject.Name = entity.Name;
            dbObject.GameStartTime = entity.GameStartTime;
            dbObject.GameEndTime = entity.GameEndTime;
            dbObject.WinnerId = entity.Winner?.Id;
            dbObject.MaxPointsCount = entity.MaxPointsCount;
            dbObject.IsActive = entity.IsActive;

            return Task.CompletedTask;
        }

        protected override IQueryable<Game> BuildDbQuery(DefaultDbContext context, IQueryable<Game> dbObjects, GamesSearchParams searchParams)
        {
            if (searchParams.IsActive.HasValue)
            {
                dbObjects = dbObjects.Where(item => item.IsActive == searchParams.IsActive.Value);
            }
            if (searchParams.WinnerId.HasValue)
            {
                dbObjects = dbObjects.Where(item => item.WinnerId == searchParams.WinnerId.Value);
            }
            if (searchParams.GameStartTime.HasValue)
            {
                dbObjects = dbObjects.Where(item => item.GameStartTime < searchParams.GameStartTime);
            }

            return dbObjects;
        }

        protected override async Task<IList<Entities.Game>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Game> dbObjects, GamesConvertParams? convertParams, bool isFull)
        {
            if (convertParams != null && convertParams.IsIncludeSessions == true)
            {
                dbObjects = dbObjects.Include(item => item.Sessions.Where(session => session.IsActive))
                    .ThenInclude(session => session.Player);
            }

            dbObjects = dbObjects.Include(item => item.Winner);

            return (await dbObjects.ToListAsync()).Select(item => ConvertDbObjectToEntity(item)).ToList();
        }

        protected override Expression<Func<Game, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        protected override Expression<Func<Entities.Game, int>> GetIdByEntityExpression()
        {
            return item => item.Id;
        }

        internal static Entities.Game ConvertDbObjectToEntity(Game dbObject, bool isIncludeAdditional = true)
        {
            if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

            return new Entities.Game(
                dbObject.Id,
                dbObject.Name,
                dbObject.GameStartTime,
                dbObject.GameEndTime,
                dbObject.Winner == null ? null : UsersDal.ConvertDbObjectToEntity(dbObject.Winner),
                dbObject.MaxPointsCount,
                dbObject.IsActive)
            {
                Sessions = isIncludeAdditional ? dbObject.Sessions.Select(SessionsDal.ConvertDbObjectToEntity).ToList() : new List<Entities.Session>()
            };
        }
    }
}
