namespace InGame.Models.Requests
{
    public class PlayerMoveRequest
    {
        public string PlayerLogin { get; set; } = string.Empty;
        public int TileId { get; set; }
        public int GameId { get; set; }
    }
}
