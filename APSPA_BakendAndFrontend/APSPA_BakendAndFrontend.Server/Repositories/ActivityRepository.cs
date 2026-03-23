using APSPA_BakendAndFrontend.Server.Data;
using APSPA_BakendAndFrontend.Server.model;
using APSPA_BakendAndFrontend.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APSPA_BakendAndFrontend.Server.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActivityRecord?> GetByIdAsync(int id) =>
            await _context.ActivityRecords.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<List<ActivityRecord>> GetByUserIdAsync(int userId) =>
            await _context.ActivityRecords
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync();

        public async Task<List<ActivityRecord>> GetAllAsync() =>
            await _context.ActivityRecords
                .Include(a => a.User)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync();

        public async Task<ActivityRecord> CreateAsync(ActivityRecord activity)
        {
            _context.ActivityRecords.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task UpdateAsync(ActivityRecord activity)
        {
            _context.ActivityRecords.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ActivityRecord activity)
        {
            _context.ActivityRecords.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
