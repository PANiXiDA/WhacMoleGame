using InGame.Models.Core;

namespace InGame.Models
{
    public class Mole : GameEntity
    {
        public int PlayerId { get; set; }
        public int TileId { get; set; }

        public Mole(Guid id, int playerId, int tileId) : base(id, "Mole")
        {
            PlayerId = playerId;
            TileId = tileId;
        }
    }
}
