namespace Dal.DbModels.Models
{
    public partial class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime GameStartTime { get; set; }
        public DateTime? GameEndTime { get; set; }
        public int? WinnerId { get; set; }
        public int? MaxPointsCount { get; set; }
        public bool IsActive { get; set; }

        public virtual User? Winner { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }

        public Game()
        {
            Sessions = new HashSet<Session>();
        }
    }
}
