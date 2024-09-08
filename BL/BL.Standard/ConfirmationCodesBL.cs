using Common.SearchParams.Core;
using Common.SearchParams;
using Dal.Interfaces;
using BL.Interfaces;
using Entities;

namespace BL.Standard
{
    public class ConfirmationCodesBL : IConfirmationCodesBL
    {
        private readonly IConfirmationCodesDal _confirmationCodesDal;

        public ConfirmationCodesBL(IConfirmationCodesDal confirmationCodesDal)
        {
            _confirmationCodesDal = confirmationCodesDal;
        }

        public async Task<int> AddOrUpdateAsync(ConfirmationCode entity)
        {
            entity.Id = await _confirmationCodesDal.AddOrUpdateAsync(entity);
            return entity.Id;
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _confirmationCodesDal.ExistsAsync(id);
        }

        public Task<bool> ExistsAsync(ConfirmationCodesSearchParams searchParams)
        {
            return _confirmationCodesDal.ExistsAsync(searchParams);
        }

        public Task<ConfirmationCode> GetAsync(int id, object? convertParams = null)
        {
            return _confirmationCodesDal.GetAsync(id, convertParams);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _confirmationCodesDal.DeleteAsync(id);
        }

        public Task<SearchResult<ConfirmationCode>> GetAsync(ConfirmationCodesSearchParams searchParams, object? convertParams = null)
        {
            return _confirmationCodesDal.GetAsync(searchParams, convertParams);
        }
    }
}
