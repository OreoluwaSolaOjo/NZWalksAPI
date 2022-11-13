using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Dormain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;
using System.Data;

namespace NZWalks.Controllers
{
    [ApiController]
    [Route("walkdifficulties")]
    public class WalkDifficultyController : Controller
    {
        private readonly IMapper mapper;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            return Ok(await walkDifficultyRepository.GetAllAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            /* Get walk domain from database*/
            var walkDifficultyDormain = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficultyDormain == null)
            {
                return NotFound();
            }
            /* Convert Dormain object to DTO*/
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDormain);
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            /* Convert Dormain to main object*/

            var walkDifficultyDormain = new Models.Dormain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            /* Pass Dormain Object to repository*/
            walkDifficultyDormain = await walkDifficultyRepository.AddAsync(walkDifficultyDormain);
            /* Convert Dormain object back to repository
             * 
                    Return Ok response*/
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficultyDormain.Code
            };
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            /* Convert dto to dormain object*/
            var walkDifficultyDormain = new Models.Dormain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code,
                
            };
            /*  pass details to repository*/

            walkDifficultyDormain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDormain);
            /*  handle not found*/
            if (walkDifficultyDormain == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficultyDormain.Code,
            };

            return Ok(walkDifficultyDTO);
            /*  Convert back dormain to dto */

        }
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            /* Get the walk from the database*/
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);
            /*if we dont get a walk send a not found back*/
            if (walkDifficulty == null)
            {
                return NotFound();
            }
            /*Convert response to DTO*/
            var walkDifficultyDTO = mapper.Map<Models.DTO.Walk>(walkDifficulty);
            /* return Ok response*/
            return Ok(walkDifficultyDTO);
        }

    }
}

