using BusinessObjects.Models;
using Repository;
using Repository.AccountRepository;
using Services.AccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Services.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService()
        {
            _feedbackRepository = new FeedbackRepository();
        }
        public async Task CreateFeedbackService(Feedback feedback)
        {   
            feedback.FeedbackId = Guid.NewGuid().ToString();
            feedback.CreatedDate = DateTime.Now;
            feedback.Status = FeedbackStatus.Pending;
            await _feedbackRepository.CreateAsync(feedback);
        }

        public async Task DeleteFeedbackService(string feedbackId)
        {
            await _feedbackRepository.DeleteAsync(feedbackId);
        }

        public async Task<Feedback> GetFeedbackDetailsService(string feedbackId)
        {
            return await _feedbackRepository.GetByIdAsync(feedbackId) ?? throw new Exception("NotFound");
        }

        public async Task<List<Feedback>> GetFeedbackPaginationService(int pageNumber, int pageSize)
        {
            return await _feedbackRepository.GetPaginationAsync(pageNumber, pageSize);
        }

        public async Task SubmitFeedbackService(string feedbackId, bool approve)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId) ?? throw new Exception("NotFound");
            feedback.Status = approve ? FeedbackStatus.Approved : FeedbackStatus.Rejected;
            feedback.SubmitedDate = DateTime.Now;
            await _feedbackRepository.UpdateAsync(feedback);
        }

        public async Task<Feedback?> UpdateFeedbackService(Feedback feedback)
        {
            await _feedbackRepository.UpdateAsync(feedback);
            return await _feedbackRepository.GetByIdAsync(feedback.FeedbackId);
        }

        public async Task<List<Feedback>> GetFeedbackPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize)
        {
            return await _feedbackRepository.GetPaginationWithSearchKeyAsync(searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumFeedbacksService()
        {
            return await _feedbackRepository.Count();
        }

        public async Task<List<Feedback>> GetFeedbackPaginationBelongUserService(string accountId, int pageNumber, int pageSize)
        {
            return await _feedbackRepository.GetPaginationBelongUserAsync(accountId, pageNumber, pageSize);
        }

        public async Task<List<Feedback>> GetFeedbackPaginationWithSearchKeyBelongUserService(string accountId, string searchKey, int pageNumber, int pageSize)
        {
            return await _feedbackRepository.GetPaginationWithSearchKeyBelongUserAsync(accountId, searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumFeedbacksBelongUserService(string accountId)
        {
            return await _feedbackRepository.CountBelongUser(accountId);
        }

        public async Task<List<Feedback>> GetFeedbackPaginationService(FeedbackStatus status, int pageNumber, int pageSize)
        {
            return await _feedbackRepository.GetPaginationAsync(status, pageNumber, pageSize);
        }

        public async Task<List<Feedback>> GetFeedbackPaginationWithSearchKeyService(FeedbackStatus status, string searchKey, int pageNumber, int pageSize)
        {
            return await _feedbackRepository.GetPaginationWithSearchKeyAsync(status, searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumFeedbacksService(FeedbackStatus status)
        {
            return await _feedbackRepository.Count(status);
        }
    }
}
