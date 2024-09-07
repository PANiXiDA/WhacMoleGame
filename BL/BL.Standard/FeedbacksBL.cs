using Common.SearchParams.Core;
using Common.SearchParams;
using Dal.Interfaces;
using BL.Interfaces;
using Entities;

namespace BL.Standard
{
    public class FeedbacksBL : IFeedbacksBL
    {
        private readonly IFeedbacksDal _feedbacksDal;

        public FeedbacksBL(IFeedbacksDal feedbacksDal)
        {
            _feedbacksDal = feedbacksDal;
        }

        public async Task<int> AddOrUpdateAsync(Feedback entity)
        {
            entity.Id = await _feedbacksDal.AddOrUpdateAsync(entity);
            return entity.Id;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _feedbacksDal.ExistsAsync(id);
        }

        public Task<bool> ExistsAsync(FeedbacksSearchParams searchParams)
        {
            return _feedbacksDal.ExistsAsync(searchParams);
        }

        public Task<Feedback> GetAsync(int id, object? convertParams = null)
        {
            return _feedbacksDal.GetAsync(id, convertParams);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _feedbacksDal.DeleteAsync(id);
        }

        public Task<SearchResult<Feedback>> GetAsync(FeedbacksSearchParams searchParams, object? convertParams = null)
        {
            return _feedbacksDal.GetAsync(searchParams, convertParams);
        }
    }
}
