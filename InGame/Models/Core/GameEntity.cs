namespace InGame.Models.Core
{
    public class GameEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public GameEntity(int id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
