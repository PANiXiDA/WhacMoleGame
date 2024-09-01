using Common.Enums;

namespace Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationDate { get; set; }

        public List<Session> Sessions { get; set; } = new List<Session>();

        public User(
            int id,
            string login,
            string password,
            string? email,
            string? phoneNumber,
            UserRole role,
            bool isBlocked,
            DateTime registrationDate)
        {
            Id = id;
            Login = login;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            Role = role;
            IsBlocked = isBlocked;
            RegistrationDate = registrationDate;
        }
    }
}
