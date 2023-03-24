using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : ControllerBase
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;
        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var walkDifficulties = await _walkDifficultyRepository.GetAllAsync();

            var walkDifficultiesDTO = _mapper.Map<List<WalkDifficultyDTO>>(walkDifficulties);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await _walkDifficultyRepository.GetAsync(id);

            if (walkDifficulty == null)
                return NotFound();

            var walkDifficultyDTO = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] CreateWalkDifficultyDTO createWalkDifficulty)
        {
            // Request(DTO) to Domain model
            var walkDifficulty = _mapper.Map<WalkDifficulty>(createWalkDifficulty);

            // Pass details to Repository
            walkDifficulty = await _walkDifficultyRepository.AddAsync(walkDifficulty);

            // Convert back to DTO
            var createdWalkDifficulty = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = createdWalkDifficulty.Id }, createdWalkDifficulty);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute]Guid id, [FromBody] UpdateWalkDifficultyDTO updateWalkDifficulty)
        {
            // Convert DTO to Domain Model
            var walkDifficulty = _mapper.Map<WalkDifficulty>(updateWalkDifficulty);

            // Update Walk Difficulty using repository
            walkDifficulty = await _walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            // If Null then NotFound
            if (walkDifficulty == null)
                return NotFound();

            // Convert Domain to back to DTO
            var updatedWalkDifficulty = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);

            //Return OK Response
            return Ok(updatedWalkDifficulty);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            // Get walk difficulty from database
            var walkDifficulty = await _walkDifficultyRepository.DeleteAsync(id);

            //If null NotFound
            if (walkDifficulty == null)
                return NotFound();

            // Convert response back to DTO
            var deletedWalkDifficulty = _mapper.Map<WalkDifficultyDTO>(walkDifficulty);

            // return Ok response
            return Ok(deletedWalkDifficulty);

        }
    }
}
