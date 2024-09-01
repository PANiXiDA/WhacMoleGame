namespace Entities
{
    public class Session
    {
        public int Id { get; set; }
        public User Player { get; set; }
        public Game Game { get; set; }
        public bool IsActive { get; set; }

        public Session(int id, User player, Game game, bool isActive)
        {
            Id = id;
            Player = player;
            Game = game;
            IsActive = isActive;
        }
    }
}
