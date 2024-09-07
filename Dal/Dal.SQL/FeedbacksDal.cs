using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces;

namespace Dal.SQL
{
    public class FeedbacksDal : BaseDal<DefaultDbContext, Feedback, Entities.Feedback, int, FeedbacksSearchParams, object>, IFeedbacksDal
    {
        protected override bool RequiresUpdatesAfterObjectSaving => false;

        public FeedbacksDal(DefaultDbContext context) : base(context) { }

        protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Feedback entity, Feedback dbObject, bool exists)
        {
            dbObject.Title = entity.Title;
            dbObject.Description = entity.Description;
            dbObject.CountStars = entity.CountStars;

            return Task.CompletedTask;
        }

        protected override IQueryable<Feedback> BuildDbQuery(DefaultDbContext context, IQueryable<Feedback> dbObjects, FeedbacksSearchParams searchParams)
        {
            return dbObjects;
        }

        protected override async Task<IList<Entities.Feedback>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Feedback> dbObjects, object? convertParams, bool isFull)
        {
            return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
        }

        protected override Expression<Func<Feedback, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        protected override Expression<Func<Entities.Feedback, int>> GetIdByEntityExpression()
        {
            return item => item.Id;
        }

        internal static Entities.Feedback ConvertDbObjectToEntity(Feedback dbObject)
        {
            if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

            return new Entities.Feedback(
                dbObject.Id,
                dbObject.Title,
                dbObject.Description,
                dbObject.CountStars);
        }
    }
}
