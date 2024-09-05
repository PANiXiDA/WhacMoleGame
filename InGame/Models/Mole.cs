using InGame.Models.Core;

namespace InGame.Models
{
    public class Mole : GameEntity
    {
        public int TileId { get; set; }

        public Mole(Guid id, int tileId) : base(id, "Mole")
        {
            TileId = tileId;
        }
    }
}
