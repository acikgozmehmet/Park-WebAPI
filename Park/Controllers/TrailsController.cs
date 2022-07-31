using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Park.Models;
using Park.Models.Dtos;
using Park.Repository.IRepository;
using System.Collections.Generic;

namespace Park.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrails();

            var objDto = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The id of the trail </param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name ="GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepository.GetTrail(trailId);

            if (obj == null)
                return NotFound();

            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Create a trail
        /// 
        /// </summary>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
                return BadRequest(ModelState);

            if(_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);
            }

            var trailObj= _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new
                    {
                        version = HttpContext.GetRequestedApiVersion().ToString(),
                        trailId = trailObj.Id
                    }, trailObj);
        }

        /// <summary>
        /// Update trail
        /// </summary>
        /// <param name="trailId"></param>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {

            if (trailDto == null || trailId != trailDto.Id)
                return BadRequest(ModelState);

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wronh when updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete trail
        /// </summary>
        /// <param name="trailId"></param>
        /// <returns></returns>

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }

            var trailObj = _trailRepository.GetTrail(trailId);

            if (!_trailRepository.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");
                return StatusCode(500,ModelState);
            }

            return NoContent();
        }


        /// <summary>
        /// Get list of trails in a national park
        /// </summary>
        /// <param name="nationalParkId">The id of the national park</param>
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailsInNationalPark(int nationalParkId)
        {
            var objList = _trailRepository.GetTrailsInNationalPark(nationalParkId);

            if (objList == null)
                return NotFound();


            var objDto = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            
            return Ok(objDto);
        }



    }
}
