namespace Dal.DbModels.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CountStars { get; set; }
    }
}
