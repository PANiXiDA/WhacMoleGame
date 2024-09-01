using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class GamesSearchParams : BaseSearchParams
    {
        public bool? IsActive { get; set; }
        public GamesSearchParams() { }
        public GamesSearchParams(int startIndex = 0, int? objectsCount = null, bool? isActive = null) : base(startIndex, objectsCount)
        {
            IsActive = isActive;
        }
    }
}
