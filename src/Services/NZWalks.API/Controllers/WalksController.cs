using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
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
    }
}
