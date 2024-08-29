using Common.Enums;
using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class UsersSearchParams : BaseSearchParams
    {
        public IEnumerable<UserRole>? Roles { get; set; }
        public UsersSearchParams() { }
        public UsersSearchParams(IEnumerable<UserRole>? roles = null, string? searchQuery = null, int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount, searchQuery)
        {
            Roles = roles;
        }
    }
}
