using Common.ConvertParams.Core;

namespace Common.ConvertParams
{
    public class UsersConvertParams : BaseConvertParams
    {
        public bool IsIncludeSessions { get; set; }
        public bool IsIncludeAllSessions { get; set; }

        public UsersConvertParams(
            bool isIncludeChildCategories = false,
            bool isIncludeParentCategories = false,
            bool isIncludeSessions = false,
            bool isIncludeAllSessions = false) : base(isIncludeChildCategories, isIncludeParentCategories)
        {
            IsIncludeSessions = isIncludeSessions;
            IsIncludeAllSessions = isIncludeAllSessions;
        }
    }
}
