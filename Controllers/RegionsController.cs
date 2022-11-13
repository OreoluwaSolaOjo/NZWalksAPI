using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NZWalks.Models.Dormain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
  /*  Add the authorize attribute to tell the the controller that you need a valid
        token to access attributes or resources of the controller */
    [ApiController]
    [Route("Regions")]
    public class RegionsController : Controller
    {
       /* Use dependency injection to pass the repository to the controller*/
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
       [HttpGet]
       [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
           var regions = await regionRepository.GetAllAsync();
            /* Created a new dto regions*/
       /*    When not using automapper*/
             /*   var regionsDTO = new List<Models.DTO.Region>();*/
          /*  regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region
                {
                    *//*All properties in the dormain DTO is copied into the region DTO*//*
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,
                };
                regionsDTO.Add(regionDTO);
            });*/
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }


        /* Define the route that gets the id of a single region*/
        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
           /* store into dormain region*/
            var region = await   regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();  
            }

          var regionDTO = mapper.Map<Models.DTO.Region>(region);
          return Ok(regionDTO); 
        }
        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validate request
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            };

            /*   Convert Request to Domain model*/
            var region = new Models.Dormain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };
            /* pass details to repository*/

            region = await regionRepository.AddAsync(region);
            /*convert back to dto*/
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }
        
        [HttpDelete]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            /* Get the region from the database*/
            var region = await regionRepository.DeleteAsync(id);
            /*if we dont get a region send a not found back*/
            if (region == null) 
            { return NotFound();
            }
            /*Convert response to DTO*/
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            /* return Ok response*/
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,[FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            /*Validate the incoming request*/
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

            /* Convert dto to domain model*//*
            manually map  not using auto mapper*/
            var region = new Models.Dormain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };
        /*    update region using repository*/
       region =  await regionRepository.UpdateAsync(id, region);
           /* if null then not found*/
           if (region == null)
            {
                return NotFound();
            }
            /* Convert dormain back to dto*/
            var regionDTO = new Models.DTO.Region()
            {
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };
            /*return Ok response*/
            return Ok(regionDTO);
        }

   /*     #region Private methods*/
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{addRegionRequest} Add region data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{addRegionRequest.Code} cannot be empty or white space");
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{addRegionRequest.Name} cannot be empty or white space");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{addRegionRequest.Area} cannot be less than or equal to zero");
            }
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{addRegionRequest.Population} cannot be less than zero");
            }
            if(ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{updateRegionRequest} Add region data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{updateRegionRequest.Code} cannot be empty or white space");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{updateRegionRequest.Name} cannot be empty or white space");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{updateRegionRequest.Area} cannot be less than or equal to zero");
            }
        
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{updateRegionRequest.Population} cannot be less than zero");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

    }
}
