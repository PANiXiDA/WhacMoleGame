using Common.SearchParams;
using User = Entities.User;
using System.Security.Cryptography;
using System.Text;
using BL.Interfaces;
using Dal.Interfaces;
using Common.SearchParams.Core;

namespace BL.Standard
{
    public class UsersBL : IUsersBL
    {
        private readonly IUsersDal _usersDal;

        public UsersBL(IUsersDal usersDal)
        {
            _usersDal = usersDal;
        }

        public async Task<int> AddOrUpdateAsync(User entity)
        {
            entity.Password = GetStringHash(entity.Password);
            entity.RegistrationDate = DateTime.Now;
            entity.Id = await _usersDal.AddOrUpdateAsync(entity);
            return entity.Id;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _usersDal.ExistsAsync(id);
        }

        public Task<bool> ExistAsync(string login)
        {
            return _usersDal.ExistsAsync(login);
        }

        public Task<bool> ExistsAsync(UsersSearchParams searchParams)
        {
            return _usersDal.ExistsAsync(searchParams);
        }

        public Task<User> GetAsync(int id, object? convertParams = null)
        {
            return _usersDal.GetAsync(id, convertParams);
        }

        public Task<User?> GetAsync(string login)
        {
            return _usersDal.GetAsync(login);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _usersDal.DeleteAsync(id);
        }

        public Task<SearchResult<User>> GetAsync(UsersSearchParams searchParams, object? convertParams = null)
        {
            return _usersDal.GetAsync(searchParams, convertParams);
        }

        public async Task<User?> VerifyPasswordAsync(string login, string password)
        {
            var user = await GetAsync(login);
            return user != null && user.Password == GetStringHash(password) ? user : null;
        }

        private string GetStringHash(string s)
        {
            if (s == null)
                return string.Empty;
            using var hashAlgorithm = SHA512.Create();
            var hash = hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(s));
            return string.Concat(hash.Select(item => item.ToString("x2")));
        }
    }
}

