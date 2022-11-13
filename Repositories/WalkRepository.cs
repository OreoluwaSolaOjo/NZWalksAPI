using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Dormain;
using NZWalks.Models.DTO;
using Walk = NZWalks.Models.Dormain.Walk;

namespace NZWalks.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            /*Assign Id*/
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
          return await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }
        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                  .Include(x => x.Region)
                  .Include(x => x.WalkDifficulty)
                  .FirstOrDefaultAsync(x => x.Id == id);
        }

       

       public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);
            if (existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalk;
            }
            return null;
        }
         async Task<Walk> IWalkRepository.DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }
            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
