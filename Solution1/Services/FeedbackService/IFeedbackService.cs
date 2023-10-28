using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Services.FeedbackService
{
    public interface IFeedbackService
    {   
        public Task CreateFeedbackService(Feedback feedback);
        public Task<Feedback?> UpdateFeedbackService(Feedback feedback);
        public Task DeleteFeedbackService(string feedbackId);
        public Task<List<Feedback>> GetFeedbackPaginationService(int pageNumber, int pageSize);
        public Task SubmitFeedbackService(string feedbackId, bool approve);
        public Task<Feedback> GetFeedbackDetailsService(string feedbackId);
        public Task<List<Feedback>> GetFeedbackPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumFeedbacksService();
        public Task<List<Feedback>> GetFeedbackPaginationBelongUserService(string accountId, int pageNumber, int pageSize);
        public Task<List<Feedback>> GetFeedbackPaginationWithSearchKeyBelongUserService(string accountId, string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumFeedbacksBelongUserService(string accountId);
        public Task<List<Feedback>> GetFeedbackPaginationService(FeedbackStatus status, int pageNumber, int pageSize);
        public Task<List<Feedback>> GetFeedbackPaginationWithSearchKeyService(FeedbackStatus status, string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumFeedbacksService(FeedbackStatus status);
    }
}

public class AssignedNote
{
    public string EmployeeId { get; set; } = null!;
    public string Note { get; set; } = null!;
}