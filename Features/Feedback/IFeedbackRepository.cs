using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Feedback
{
    public interface IFeedbackRepository
    {
        // CRUD Operations
        Task<Entities.Feedback?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Feedback>> GetAllAsync();
        Task<Entities.Feedback> CreateAsync(Entities.Feedback feedback);
        Task<Entities.Feedback> UpdateAsync(Entities.Feedback feedback);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        
        // Feedback-specific operations
        Task<IEnumerable<Entities.Feedback>> GetActiveFeedbacksAsync();
        Task<IEnumerable<Entities.Feedback>> GetByRatingAsync(int rating);
        Task<IEnumerable<Entities.Feedback>> GetByPositionAsync(string position);
        Task<int> GetTotalCountAsync();
        Task<int> GetActiveCountAsync();
        Task<bool> ExistsAsync(int id);
        Task<double> GetAverageRatingAsync();
    }
} 