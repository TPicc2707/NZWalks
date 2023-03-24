using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public WalkDifficultyRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            // Assign New Id
            walkDifficulty.Id = Guid.NewGuid();
            await _dbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await _dbContext.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty == null)
                return null;

            //Delete the walk
            _dbContext.WalkDifficulty.Remove(existingWalkDifficulty);
            await _dbContext.SaveChangesAsync();
            return existingWalkDifficulty;

        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _dbContext.WalkDifficulty.ToListAsync();

        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _dbContext.WalkDifficulty.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await _dbContext.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty != null)
            {
                existingWalkDifficulty.Code = walkDifficulty.Code;
                await _dbContext.SaveChangesAsync();

                return existingWalkDifficulty;
            }

            return null;
        }
    }
}
