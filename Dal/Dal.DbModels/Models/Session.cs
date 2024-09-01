namespace Dal.DbModels.Models
{
    public partial class Session
    {
        public int Id { get; set; }
        public int PlayerId {  get; set; }
        public int GameId {  get; set; }
        public bool IsActive { get; set; }

        public virtual User? Player { get; set; }
        public virtual Game? Game { get; set; }
    }
}
