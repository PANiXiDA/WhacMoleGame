namespace Dal.DbModels.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }

        public User()
        {
            Sessions = new HashSet<Session>();
        }
    }
}