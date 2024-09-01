using Common.ConvertParams.Core;

namespace Common.ConvertParams
{
    public class UsersConvertParams : BaseConvertParams
    {
        public bool IsIncludeSessions { get; set; }
        public UsersConvertParams(
            bool isIncludeChildCategories = false,
            bool isIncludeParentCategories = false,
            bool isIncludeSessions = false) : base(isIncludeChildCategories, isIncludeParentCategories)
        {
            IsIncludeSessions = isIncludeSessions;
        }
    }
}
