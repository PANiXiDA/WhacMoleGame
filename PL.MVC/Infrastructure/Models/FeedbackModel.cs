using Entities;

namespace PL.MVC.Infrastructure.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CountStars { get; set; }

        public static FeedbackModel FromEntity(Feedback obj)
        {
            return new FeedbackModel
            {
                Id = obj.Id,
                Title = obj.Title,
                Description = obj.Description,
                CountStars = obj.CountStars
            };
        }

        public static Feedback ToEntity(FeedbackModel obj)
        {
            return new Feedback(
                obj.Id,
                obj.Title,
                obj.Description,
                obj.CountStars);
        }

        public static List<FeedbackModel> FromEntitiesList(IEnumerable<Feedback> list)
        {
            return list?.Select(FromEntity).Where(x => x != null).Cast<FeedbackModel>().ToList() ?? new List<FeedbackModel>();
        }

        public static List<Feedback> ToEntitiesList(IEnumerable<FeedbackModel> list)
        {
            return list?.Select(ToEntity).Where(x => x != null).Cast<Feedback>().ToList() ?? new List<Feedback>();
        }
    }
}
