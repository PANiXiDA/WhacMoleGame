using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces.Core;

namespace Dal.Interfaces
{
    public interface IFeedbacksDal : IBaseDal<DefaultDbContext, Feedback, Entities.Feedback, int, FeedbacksSearchParams, object>
    {
    }
}
