using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.DTO;
/*using NZWalks.Models.DTO;*/
using NZWalks.Repositories;
using System.Data;


namespace NZWalks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalksAsync()
        {

            /* Fetch data from database - domain walks*/
            var walksDormain = await walkRepository.GetAllAsync();
            /* Convert domain walks to dto walks*/
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDormain);
            /* return response*/
            return Ok(walksDTO);


        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            /* Get walk domain from database*/
            var walkDormain = await walkRepository.GetAsync(id);
            /* Convert Dormain object to DTO*/
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDormain);
            return Ok(walkDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addwalkRequest)
        {/*
            Validate the incoming request
*/
            if (ValidateAddWalkAsync(addwalkRequest))
            {
                return BadRequest(ModelState);
            };
            /* Convert Dormain to main object*/

            var walkDormain = new Models.Dormain.Walk
            {
                Length = addwalkRequest.Length,
                Name = addwalkRequest.Name,
                RegionId = addwalkRequest.RegionId,
                WalkDifficultyId = addwalkRequest.WalkDifficultyId,
            };
            /* Pass Dormain Object to repository*/
            walkDormain = await walkRepository.AddAsync(walkDormain);
            /* Convert Dormain object back to repository
             * 
                    Return Ok response*/
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDormain.Id,
                Length = walkDormain.Length,
                Name = walkDormain.Name,
                RegionId = walkDormain.RegionId,
                WalkDifficultyId = walkDormain.WalkDifficultyId,


            };

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            /* Convert dto to dormain object*/
            var walkDormain = new Models.Dormain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };
            /*  pass details to repository*/

            walkDormain = await walkRepository.UpdateAsync(id, walkDormain);
            /*  handle not found*/
            if (walkDormain == null)
            {
                return NotFound();
            }

            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDormain.Id,
                Length = walkDormain.Length,
                Name = walkDormain.Name,
                RegionId = walkDormain.RegionId,
                WalkDifficultyId = walkDormain.WalkDifficultyId,
            };

            return Ok(walkDTO);
            /*  Convert back dormain to dto */

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            /* Get the walk from the database*/
            var walk = await walkRepository.DeleteAsync(id);
            /*if we dont get a walk send a not found back*/
            if (walk == null)
            {
                return NotFound();
            }
            /*Convert response to DTO*/
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            /* return Ok response*/
            return Ok(walkDTO);
        }

        private bool ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest )
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{addWalkRequest} Add walk data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{addWalkRequest.Name} cannot be empty or white space");
            }
         
            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{addWalkRequest.Length} cannot be less than or equal to zero");
            }
           /* To check for region id since it is a guid and fk*/
            var region = regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region == null)
            {
               ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{addWalkRequest.RegionId} is invalid");
            };

           /* do similar for walk difficulty id as for region id*/
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
    }
}

