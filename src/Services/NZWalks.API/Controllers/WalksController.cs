using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, 
            IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository, 
            IMapper mapper)
        {
            _walkRepository = walkRepository;
            _regionRepository = regionRepository;
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await _walkRepository.GetAllAsync();

            var walkDTO = _mapper.Map<List<WalkDTO>>(walks);

            return Ok(walkDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await _walkRepository.GetAsync(id);

            if (walk == null)
                return NotFound();

            var walkDTO = _mapper.Map<WalkDTO>(walk);

            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] CreateWalkDTO createWalk)
        {
            //Validate the incoming request
            if (!(await ValidateCreateWalkAsync(createWalk)))
                return BadRequest(ModelState);

            // Request(DTO) to Domain model
            var walk = _mapper.Map<Walk>(createWalk);

            // Pass details to Repository
            walk = await _walkRepository.AddAsync(walk);

            // Convert back to DTO
            var createdWalk = _mapper.Map<WalkDTO>(walk);

            return CreatedAtAction(nameof(GetWalkAsync), new { id = createdWalk.Id }, createdWalk);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalk)
        {
            //Validate the incoming request
            if (!(await ValidateUpdateWalkAsync(updateWalk)))
                return BadRequest(ModelState);


            // Convert DTO to Domain Model
            var walk = _mapper.Map<Walk>(updateWalk);

            // Update Walk using repository
            walk = await _walkRepository.UpdateAsync(id, walk);

            // If Null then NotFound
            if (walk == null)
                return NotFound();

            // Convert Domain to back to DTO
            var updatedWalk = _mapper.Map<WalkDTO>(walk);

            //Return OK Response
            return Ok(updatedWalk);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            // Get walk from database
            var walk = await _walkRepository.DeleteAsync(id);

            //If null NotFound
            if (walk == null)
                return NotFound();

            // Convert response back to DTO
            var deletedWalk = _mapper.Map<WalkDTO>(walk);

            // return Ok response
            return Ok(deletedWalk);

        }

        #region Private Methods

        private async Task<bool> ValidateCreateWalkAsync(CreateWalkDTO createWalkDTO)
        {
            if (createWalkDTO == null)
            {
                ModelState.AddModelError(nameof(createWalkDTO),
                    $"{nameof(createWalkDTO)} cannot be empty.");
                return false;

            }
            if (string.IsNullOrWhiteSpace(createWalkDTO.Name))
            {
                ModelState.AddModelError(nameof(createWalkDTO.Name),
                    $"{nameof(createWalkDTO.Name)} is required.");
            }

            if (createWalkDTO.Length <= 0)
            {
                ModelState.AddModelError(nameof(createWalkDTO.Length),
                    $"{nameof(createWalkDTO.Length)} should be greater than zero.");
            }

            var region = await _regionRepository.GetAsync(createWalkDTO.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(createWalkDTO.RegionId),
                    $"{nameof(createWalkDTO.RegionId)} is invalid.");
            }

            var walkDifficulty = await _walkDifficultyRepository.GetAsync(createWalkDTO.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(createWalkDTO.WalkDifficultyId),
                    $"{nameof(createWalkDTO.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;

        }

        private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkDTO updateWalkDTO)
        {
            if (updateWalkDTO == null)
            {
                ModelState.AddModelError(nameof(updateWalkDTO),
                    $"{nameof(updateWalkDTO)} cannot be empty.");
                return false;

            }
            if (string.IsNullOrWhiteSpace(updateWalkDTO.Name))
            {
                ModelState.AddModelError(nameof(updateWalkDTO.Name),
                    $"{nameof(updateWalkDTO.Name)} is required.");
            }

            if (updateWalkDTO.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkDTO.Length),
                    $"{nameof(updateWalkDTO.Length)} should be greater than zero.");
            }

            var region = await _regionRepository.GetAsync(updateWalkDTO.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkDTO.RegionId),
                    $"{nameof(updateWalkDTO.RegionId)} is invalid.");
            }

            var walkDifficulty = await _walkDifficultyRepository.GetAsync(updateWalkDTO.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkDTO.WalkDifficultyId),
                    $"{nameof(updateWalkDTO.WalkDifficultyId)} is invalid.");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;

        }

        #endregion
    }
}
