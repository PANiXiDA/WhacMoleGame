using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class ConfirmationCodesSearchParams : BaseSearchParams
    {
        public ConfirmationCodesSearchParams() { }
        public ConfirmationCodesSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
        {
        }
    }
}
