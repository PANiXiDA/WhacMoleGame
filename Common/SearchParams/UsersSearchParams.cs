using Common.Enums;
using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class UsersSearchParams : BaseSearchParams
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber {  get; set; }
        public IEnumerable<UserRole>? Roles { get; set; }
        public UsersSearchParams() { }
        public UsersSearchParams(
            IEnumerable<UserRole>? roles = null,
            string? login = null,
            string? email = null,
            string? phoneNumber = null,
            string? searchQuery = null,
            int startIndex = 0,
            int? objectsCount = null) : base(startIndex, objectsCount, searchQuery)
        {
            Roles = roles;
            Login = login;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
