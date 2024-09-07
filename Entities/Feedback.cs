namespace Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CountStars {  get; set; }

        public Feedback(int id, string title, string description, int countStars)
        {
            Id = id;
            Title = title;
            Description = description;
            CountStars = countStars;
        }
    }
}
