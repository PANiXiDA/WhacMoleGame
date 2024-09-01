namespace Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime GameStartTime { get; set; }
        public DateTime? GameEndTime { get; set; }
        public User? Winner { get; set; }
        public int? MaxPointsCount { get; set; }
        public bool IsActive { get; set; }

        public List<Session>? Sessions { get; set; }

        public Game(
            int id,
            string name,
            DateTime gameStartTime,
            DateTime? gameEndTime,
            User? winner,
            int? maxPointsCount,
            bool isActive,
            List<Session>? sessions = null)
        {
            Id = id;
            Name = name;
            GameStartTime = gameStartTime;
            GameEndTime = gameEndTime;
            Winner = winner;
            MaxPointsCount = maxPointsCount;
            IsActive = isActive;
            Sessions = sessions;
        }
    }
}
