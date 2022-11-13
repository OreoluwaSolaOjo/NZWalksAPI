using NZWalks.Models.Dormain;

namespace NZWalks.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();

        Task<Region> GetAsync(Guid  Id);

        Task<Region> AddAsync(Region region);

        Task<Region> DeleteAsync(Guid id);

        /*update takes in the id and the region*/
        Task<Region> UpdateAsync(Guid id, Region region);
    }
}
