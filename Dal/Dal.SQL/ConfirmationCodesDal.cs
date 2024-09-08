using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces;

namespace Dal.SQL
{
    public class ConfirmationCodesDal : BaseDal<DefaultDbContext, ConfirmationCode, Entities.ConfirmationCode, int, ConfirmationCodesSearchParams, object>, IConfirmationCodesDal
    {
        protected override bool RequiresUpdatesAfterObjectSaving => false;

        public ConfirmationCodesDal(DefaultDbContext context) : base(context) { }

        protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.ConfirmationCode entity, ConfirmationCode dbObject, bool exists)
        {
            dbObject.Code = entity.Code;

            return Task.CompletedTask;
        }

        protected override IQueryable<ConfirmationCode> BuildDbQuery(DefaultDbContext context, IQueryable<ConfirmationCode> dbObjects, ConfirmationCodesSearchParams searchParams)
        {
            return dbObjects;
        }

        protected override async Task<IList<Entities.ConfirmationCode>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<ConfirmationCode> dbObjects, object? convertParams, bool isFull)
        {
            return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
        }

        protected override Expression<Func<ConfirmationCode, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        protected override Expression<Func<Entities.ConfirmationCode, int>> GetIdByEntityExpression()
        {
            return item => item.Id;
        }

        internal static Entities.ConfirmationCode ConvertDbObjectToEntity(ConfirmationCode dbObject)
        {
            if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

            return new Entities.ConfirmationCode(
                dbObject.Id,
                dbObject.Code);
        }
    }
}
