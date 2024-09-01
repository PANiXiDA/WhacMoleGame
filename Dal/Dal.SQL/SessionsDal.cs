using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces;

namespace Dal.SQL
{
    public class SessionsDal : BaseDal<DefaultDbContext, Session, Entities.Session, int, SessionsSearchParams, object>, ISessionsDal
    {
        protected override bool RequiresUpdatesAfterObjectSaving => false;

        public SessionsDal(DefaultDbContext context) : base(context) { }

        protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Session entity, Session dbObject, bool exists)
        {
            dbObject.PlayerId = entity.Player.Id;
            dbObject.GameId = entity.Game.Id;
            dbObject.IsActive = entity.IsActive;

            return Task.CompletedTask;
        }

        protected override IQueryable<Session> BuildDbQuery(DefaultDbContext context, IQueryable<Session> dbObjects, SessionsSearchParams searchParams)
        {
            if (searchParams.PlayerId.HasValue)
            {
                dbObjects = dbObjects.Where(item => item.PlayerId == searchParams.PlayerId.Value);
            }
            if (searchParams.GameId.HasValue)
            {
                dbObjects = dbObjects.Where(item => item.GameId == searchParams.GameId.Value);
            }

            dbObjects = dbObjects.Where(item => item.IsActive);

            return dbObjects;
        }

        protected override async Task<IList<Entities.Session>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Session> dbObjects, object? convertParams, bool isFull)
        {
            return (await dbObjects.Include(item => item.Player).Include(item => item.Game).ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
        }

        protected override Expression<Func<Session, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        protected override Expression<Func<Entities.Session, int>> GetIdByEntityExpression()
        {
            return item => item.Id;
        }

        internal static Entities.Session ConvertDbObjectToEntity(Session dbObject)
        {
            if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

            var player = dbObject.Player != null
                ? UsersDal.ConvertDbObjectToEntity(dbObject.Player, false)
                : throw new InvalidOperationException("Player cannot be null");

            var game = dbObject.Game != null
                ? GamesDal.ConvertDbObjectToEntity(dbObject.Game, false)
                : throw new InvalidOperationException("Game cannot be null");

            return new Entities.Session(
                dbObject.Id,
                player,
                game,
                dbObject.IsActive
            );
        }
    }
}
