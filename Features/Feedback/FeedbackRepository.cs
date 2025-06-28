using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Feedback
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedbackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
        }

        public async Task<IEnumerable<Entities.Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks
                .Where(f => !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<Entities.Feedback> CreateAsync(Entities.Feedback feedback)
        {
            feedback.CreatedAt = DateTime.UtcNow;
            feedback.IsActive = true;
            feedback.IsDeleted = false;
            
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<Entities.Feedback> UpdateAsync(Entities.Feedback feedback)
        {
            feedback.LastModifiedAt = DateTime.UtcNow;
            
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
                return false;

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
                return false;

            feedback.IsDeleted = true;
            feedback.DeletedAt = DateTime.UtcNow;
            feedback.IsActive = false;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.Feedback>> GetActiveFeedbacksAsync()
        {
            return await _context.Feedbacks
                .Where(f => !f.IsDeleted && f.IsActive)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Feedback>> GetByRatingAsync(int rating)
        {
            return await _context.Feedbacks
                .Where(f => !f.IsDeleted && f.Rating == rating)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Feedback>> GetByPositionAsync(string position)
        {
            return await _context.Feedbacks
                .Where(f => !f.IsDeleted && f.Position.ToLower().Contains(position.ToLower()))
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Feedbacks.CountAsync(f => !f.IsDeleted);
        }

        public async Task<int> GetActiveCountAsync()
        {
            return await _context.Feedbacks.CountAsync(f => !f.IsDeleted && f.IsActive);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Feedbacks.AnyAsync(f => f.Id == id && !f.IsDeleted);
        }

        public async Task<double> GetAverageRatingAsync()
        {
            return await _context.Feedbacks
                .Where(f => !f.IsDeleted && f.IsActive)
                .AverageAsync(f => f.Rating);
        }
    }
} 