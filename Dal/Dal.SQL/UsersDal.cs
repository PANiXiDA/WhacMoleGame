using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Common.Enums;
using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces;

namespace Dal.SQL
{
    public class UsersDal : BaseDal<DefaultDbContext, User, Entities.User, int, UsersSearchParams, object>, IUsersDal
    {
        protected override bool RequiresUpdatesAfterObjectSaving => false;

        public UsersDal(DefaultDbContext context) : base(context) { }

        protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.User entity, User dbObject, bool exists)
        {
            dbObject.Login = entity.Login;
            dbObject.Password = entity.Password;
            dbObject.RoleId = (int)entity.Role;
            dbObject.Email = entity.Email;
            dbObject.PhoneNumber = entity.PhoneNumber;
            dbObject.IsBlocked = entity.IsBlocked;
            dbObject.RegistrationDate = entity.RegistrationDate;

            return Task.CompletedTask;
        }

        protected override IQueryable<User> BuildDbQuery(DefaultDbContext context, IQueryable<User> dbObjects, UsersSearchParams searchParams)
        {
            if (searchParams.Roles != null)
            {
                var rolesArray = searchParams.Roles.Cast<int>().ToArray();
                dbObjects = dbObjects.Where(item => rolesArray.Contains(item.RoleId));
            }
            if (searchParams.Login != null)
            {
                dbObjects = dbObjects.Where(item => item.Login == searchParams.Login);
            }
            if (searchParams.Email != null)
            {
                dbObjects = dbObjects.Where(item => item.Email == searchParams.Email);
            }
            if (searchParams.PhoneNumber != null)
            {
                dbObjects = dbObjects.Where(item => item.PhoneNumber == searchParams.PhoneNumber);
            }

            if (!string.IsNullOrEmpty(searchParams.SearchQuery))
            {
                dbObjects = dbObjects.Where(item => item.Login.Contains(searchParams.SearchQuery));
            }

            dbObjects = dbObjects.OrderBy(item => item.Login);

            return dbObjects;
        }

        protected override async Task<IList<Entities.User>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<User> dbObjects, object? convertParams, bool isFull)
        {
            return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
        }

        protected override Expression<Func<User, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        protected override Expression<Func<Entities.User, int>> GetIdByEntityExpression()
        {
            return item => item.Id;
        }

        internal static Entities.User ConvertDbObjectToEntity(User dbObject)
        {
            if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

            return new Entities.User(
                dbObject.Id,
                dbObject.Login,
                dbObject.Password,
                dbObject.Email,
                dbObject.PhoneNumber,
                (UserRole)dbObject.RoleId,
                dbObject.IsBlocked,
                dbObject.RegistrationDate
            );
        }

        public Task<bool> ExistsAsync(string login)
        {
            return ExistsAsync(item => item.Login == login);
        }

        public async Task<Entities.User?> GetAsync(string login)
        {
            return (await GetAsync(item => item.Login == login)).FirstOrDefault();
        }
    }
}
