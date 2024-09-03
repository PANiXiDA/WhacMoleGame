using Common.Enums;
using PL.MVC.Infrastructure.Models;
using System.Security.Claims;

namespace PL.MVC.Infrastructure.Claims
{
    public class CustomUserIdentity : ClaimsIdentity
    {
        public int? Id { get; set; }

        public CustomUserIdentity(UserModel user, string authenticationType = "Cookie") : base(GetUserClaims(user), authenticationType)
        {
            Id = user.Id;
        }
        private static List<Claim> GetUserClaims(UserModel user)
        {
            if (user == null)
            {
                return new List<Claim>();
            }

            var result = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.Id.ToString())
            };
            return result;
        }

        public CustomUserIdentity(int userId, string userLogin, UserRole userRole, string authenticationType = "Cookie")
            : base(GetUserClaims(userId, userLogin, userRole), authenticationType)
        {
            Id = userId;
        }
        private static List<Claim> GetUserClaims(int userId, string userLogin, UserRole userRole)
        {
            if (userLogin == null)
            {
                return new List<Claim>();
            }
            var result = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userLogin),
                new Claim(ClaimTypes.Role, userRole.ToString()),
                new Claim("UserId", userId.ToString())
            };
            return result;
        }
    }
}
