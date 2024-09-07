using Common.SearchParams;
using Entities;

namespace BL.Interfaces
{
    public interface IFeedbacksBL : ICrudBL<Feedback, FeedbacksSearchParams, object>
    {
    }
}
