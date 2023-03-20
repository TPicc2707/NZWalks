using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public WalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assign New Id
            walk.Id = Guid.NewGuid();
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await _dbContext.Walks.FindAsync(id);

            if (existingWalk == null)
                return null;

            //Delete the walk
            _dbContext.Walks.Remove(existingWalk);
            await _dbContext.SaveChangesAsync();
            return existingWalk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _dbContext.Walks.Include(w => w.Region).Include(w => w.WalkDifficulty).ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await _dbContext.Walks.Include(w => w.Region).Include(w => w.WalkDifficulty).FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _dbContext.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await _dbContext.SaveChangesAsync();

                return existingWalk;
            }

            return null;
        }
    }
}
