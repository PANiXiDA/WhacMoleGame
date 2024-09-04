using InGame.Models.Core;

namespace InGame.Models
{
    public class Plant : GameEntity
    {
        public int TileId { get; set; }

        public Plant(Guid id, int tileId) : base(id, "Plant")
        {
            TileId = tileId;
        }
    }
}
