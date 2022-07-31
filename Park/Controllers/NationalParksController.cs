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
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var objList = _nationalParkRepository.GetNationalParks();

            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId">The id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name ="GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _nationalParkRepository.GetNationalPark(nationalParkId);

            if (obj == null)
                return NotFound();

            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Create a national park
        /// 
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
                return BadRequest(ModelState);

            if(_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wronh when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark",new { version=HttpContext.GetRequestedApiVersion().ToString(),
                                                          nationalParkId = nationalParkObj.Id}, nationalParkObj);
        }

        /// <summary>
        /// Update national park
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {

            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
                return BadRequest(ModelState);

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nationalParkRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wronh when updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete national park
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(500)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkObj = _nationalParkRepository.GetNationalPark(nationalParkId);

            if (!_nationalParkRepository.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");
                return StatusCode(500,ModelState);
            }

            return NoContent();
        }
    }
}
