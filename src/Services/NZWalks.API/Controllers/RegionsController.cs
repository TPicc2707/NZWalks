using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();

            // return DTO regions
            //var regionsDTO = new List<RegionDTO>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new RegionDTO()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population
            //    };

            //    regionsDTO.Add(regionDTO);
            //});

            var regionsDTO = _mapper.Map<IEnumerable<RegionDTO>>(regions);

            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetAsync(id);

            if (region == null)
                return NotFound();

            var regionDTO = _mapper.Map<RegionDTO>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync([FromBody]CreateRegionDTO createRegion)
        {
            // Validate The Request
            if (!ValidateCreateRegionAsync(createRegion))
            {
                return BadRequest(ModelState);
            }

            // Request(DTO) to Domain model
            var region = _mapper.Map<Region>(createRegion);

            // Pass details to Repository
            region = await _regionRepository.AddAsync(region);

            // Convert back to DTO
            var createdRegion = _mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(GetRegionAsync), new { id = createdRegion.Id}, createdRegion);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get region from database
            var region = await _regionRepository.DeleteAsync(id);

            //If null NotFound
            if (region == null)
                return NotFound();

            // Convert response back to DTO
            var deletedRegion = _mapper.Map<RegionDTO>(region);

            // return Ok response
            return Ok(deletedRegion);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]UpdateRegionDTO updateRegion)
        {
            // Validate The Request
            if (!ValidateUpdateRegionAsync(updateRegion))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain Model
            var region = _mapper.Map<Region>(updateRegion);

            // Update Region using repository
            region = await _regionRepository.UpdateAsync(id, region);

            // If Null then NotFound
            if (region == null)
                return NotFound();

            // Convert Domain to back to DTO
            var updatedRegion = _mapper.Map<RegionDTO>(region);

            //Return OK Response
            return Ok(updatedRegion);
        }

        #region Private methods

        private bool ValidateCreateRegionAsync(CreateRegionDTO createRegionDTO)
        {
            if(createRegionDTO == null)
            {
                ModelState.AddModelError(nameof(createRegionDTO),
                    $"Create Region Data is required.");
                return false;

            }
            if (string.IsNullOrWhiteSpace(createRegionDTO.Code))
            {
                ModelState.AddModelError(nameof(createRegionDTO.Code),
                    $"{nameof(createRegionDTO.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(createRegionDTO.Name))
            {
                ModelState.AddModelError(nameof(createRegionDTO.Name),
                    $"{nameof(createRegionDTO.Name)} cannot be null or empty or white space.");
            }

            if (createRegionDTO.Area <= 0)
            {
                ModelState.AddModelError(nameof(createRegionDTO.Area),
                    $"{nameof(createRegionDTO.Area)} cannot be less than or equal to zero.");
            }

            if (createRegionDTO.Population < 0)
            {
                ModelState.AddModelError(nameof(createRegionDTO.Population),
                    $"{nameof(createRegionDTO.Population)} cannot be less than or equal to zero.");
            }

            if(ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateRegionAsync(UpdateRegionDTO updateRegionDTO)
        {
            if (updateRegionDTO == null)
            {
                ModelState.AddModelError(nameof(updateRegionDTO),
                    $"Update Region Data is required.");
                return false;

            }

            if (string.IsNullOrWhiteSpace(updateRegionDTO.Code))
            {
                ModelState.AddModelError(nameof(updateRegionDTO.Code),
                    $"{nameof(updateRegionDTO.Code)} cannot be null or empty or white space.");
            }

            if (string.IsNullOrWhiteSpace(updateRegionDTO.Name))
            {
                ModelState.AddModelError(nameof(updateRegionDTO.Name),
                    $"{nameof(updateRegionDTO.Name)} cannot be null or empty or white space.");
            }

            if (updateRegionDTO.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionDTO.Area),
                    $"{nameof(updateRegionDTO.Area)} cannot be less than or equal to zero.");
            }

            if (updateRegionDTO.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionDTO.Population),
                    $"{nameof(updateRegionDTO.Population)} cannot be less than or equal to zero.");
            }

            if (ModelState.ErrorCount > 0)
                return false;

            return true;

        }

        #endregion

    }
}
