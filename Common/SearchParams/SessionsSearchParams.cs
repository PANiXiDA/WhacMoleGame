using Common.SearchParams.Core;

namespace Common.SearchParams
{
    public class SessionsSearchParams : BaseSearchParams
    {
        public int? PlayerId {  get; set; }
        public int? GameId { get; set; }
        public SessionsSearchParams() { }
        public SessionsSearchParams(
            int startIndex = 0,
            int? objectsCount = null,
            int? playerId = null,
            int? gameId = null) : base(startIndex, objectsCount)
        {
            PlayerId = playerId;
            GameId = gameId;
        }
    }
}
