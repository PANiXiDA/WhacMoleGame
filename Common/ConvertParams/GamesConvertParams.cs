using Common.ConvertParams.Core;

namespace Common.ConvertParams
{
    public class GamesConvertParams: BaseConvertParams
    {
        public bool IsIncludeSessions { get; set; }
        public GamesConvertParams(
            bool isIncludeChildCategories = false,
            bool isIncludeParentCategories = false,
            bool isIncludeSessions = false) : base(isIncludeChildCategories, isIncludeParentCategories)
        {
            IsIncludeSessions = isIncludeSessions;
        }
    }
}
