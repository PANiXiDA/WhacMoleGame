using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class FeedbacksSearchParams : BaseSearchParams
    {
        public FeedbacksSearchParams() { }
        public FeedbacksSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
        {
        }
    }
}
