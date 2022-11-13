﻿using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Dormain;

namespace NZWalks.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public RegionRepository(NZWalksDbContext nZWalksDbContext){
            this.nZWalksDbContext = nZWalksDbContext;
}

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
           await nZWalksDbContext.AddAsync(region);
           await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid Id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id ==Id);
        }

         async Task<Region> IRegionRepository.DeleteAsync(Guid id)
        {
            var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(region == null)
            {
                return null;
            }
            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }
        async Task<Region> IRegionRepository.UpdateAsync(Guid id, Region region)
        {
           var existingRegion = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null; 
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Code;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

            await nZWalksDbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}