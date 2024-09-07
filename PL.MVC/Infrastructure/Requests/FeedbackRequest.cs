namespace PL.MVC.Infrastructure.Requests
{
    public class FeedbackRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CountStars { get; set; }
    }
}
