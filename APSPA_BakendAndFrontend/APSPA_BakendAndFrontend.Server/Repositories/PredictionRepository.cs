using APSPA_BakendAndFrontend.Server.Data;
using APSPA_BakendAndFrontend.Server.model;
using APSPA_BakendAndFrontend.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APSPA_BakendAndFrontend.Server.Repositories
{
    public class PredictionRepository : IPredictionRepository
    {
        private readonly AppDbContext _context;

        public PredictionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Prediction?> GetByIdAsync(int id) =>
            await _context.Predictions
                .Include(p => p.AiRequestLogs)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Prediction>> GetByUserIdAsync(int userId) =>
            await _context.Predictions
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

        public async Task<List<Prediction>> GetByActivityIdAsync(int activityId) =>
            await _context.Predictions
                .Where(p => p.ActivityRecordId == activityId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

        public async Task<List<Prediction>> GetAllAsync() =>
            await _context.Predictions
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
    }
}
