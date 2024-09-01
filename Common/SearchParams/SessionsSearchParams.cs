using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class SessionsSearchParams : BaseSearchParams
    {
        public SessionsSearchParams() { }
        public SessionsSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount) { }
    }
}
