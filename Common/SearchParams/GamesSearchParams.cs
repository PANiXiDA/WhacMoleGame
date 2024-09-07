using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class GamesSearchParams : BaseSearchParams
    {
        public bool? IsActive { get; set; }
        public int? WinnerId { get; set; }
        public DateTime? GameStartTime { get; set; }

        public GamesSearchParams() { }
        public GamesSearchParams(
            int startIndex = 0,
            int? objectsCount = null,
            bool? isActive = null,
            int? winnerId = null,
            DateTime gameStartTime = default) : base(startIndex, objectsCount)
        {
            IsActive = isActive;
            WinnerId = winnerId;
            GameStartTime = gameStartTime;
        }
    }
}
