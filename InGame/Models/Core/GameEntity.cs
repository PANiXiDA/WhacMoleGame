namespace InGame.Models.Core
{
    public class GameEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; }

        public GameEntity(Guid id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
