using Common.Enums;
using Entities;

namespace PL.MVC.Infrastructure.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<SessionModel> Sessions { get; set; } = new List<SessionModel>();

        public static UserModel FromEntity(User obj)
        {
            return new UserModel
            {
                Id = obj.Id,
                Login = obj.Login,
                Password = obj.Password,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Role = obj.Role,
                IsBlocked = obj.IsBlocked,
                RegistrationDate = obj.RegistrationDate,
                Sessions = SessionModel.FromEntitiesList(obj.Sessions)
            };
        }

        public static User ToEntity(UserModel obj)
        {
            return new User(
                obj.Id,
                obj.Login,
                obj.Password,
                obj.Email,
                obj.PhoneNumber,
                obj.Role,
                obj.IsBlocked,
                obj.RegistrationDate);
        }

        public static List<UserModel> FromEntitiesList(IEnumerable<User> list)
        {
            return list?.Select(FromEntity).Where(x => x != null).Cast<UserModel>().ToList() ?? new List<UserModel>();
        }

        public static List<User> ToEntitiesList(IEnumerable<UserModel> list)
        {
            return list?.Select(ToEntity).Where(x => x != null).Cast<User>().ToList() ?? new List<User>();
        }
    }
}
