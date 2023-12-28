using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StreamberryMoviesApi.Data;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StreamingPlatformController : ControllerBase
    {
        private StreamBerryContext _context;
        private IMapper _mapper;
        public StreamingPlatformController(StreamBerryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new streaming platform to the database.
        /// </summary>
        /// <param name="streamingPlatformDto">Object with necessary fields to create a streaming platform.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Returns the newly created streaming platform.</response>
        /// <response code="409">If the streaming platform already exists.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult AddStreamingPlatform([FromBody] CreateStreamingPlatformDto streamingPlatformDto)
        {
            if (FindStreamingPlatformByName(streamingPlatformDto.Name))
            {
                return Conflict("Streaming Platform already exists");
            }

            StreamingPlatform streamingPlatform = _mapper.Map<StreamingPlatform>(streamingPlatformDto);
            _context.StreamingServices.Add(streamingPlatform);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetStreamingPlatformById), new { id = streamingPlatform.Id }, streamingPlatform);
        }

        private bool FindStreamingPlatformByName(string name)
        {
            bool streamingExists = _context.StreamingServices.Any(sp => sp.Name == name);

            if (streamingExists)
                return true;

            return false;
        }

        /// <summary>
        /// Retrieves a list of streaming platforms with optional pagination.
        /// </summary>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of streaming platforms.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ReadStreamingPlatformDto> GetAllStreamingPlatforms([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadStreamingPlatformDto>>(_context.StreamingServices.Skip(skip).Take(take).ToList());
        }

        /// <summary>
        /// Retrieves a specific streaming platform by ID.
        /// </summary>
        /// <param name="id">Identifier of the streaming platform to be retrieved.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the streaming platform with the specified ID is not found.</response>
        /// <response code="200">Returns the requested streaming platform.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetStreamingPlatformById(int id)
        {
            var streamingPlatform = _context.StreamingServices.FirstOrDefault(movie => movie.Id == id);

            if (streamingPlatform == null)
                return NotFound();

            var streamingPlatformDto = _mapper.Map<ReadStreamingPlatformDto>(streamingPlatform);

            return Ok(streamingPlatformDto);
        }

        /// <summary>
        /// Updates a streaming platform by ID.
        /// </summary>
        /// <param name="id">Identifier of the streaming platform to be updated.</param>
        /// <param name="streamingPlatformDto">Object with necessary fields to update a streaming platform.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the streaming platform with the specified ID is not found.</response>
        /// <response code="204">If the update was successful.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateStreamingPlatformById(int id, [FromBody] UpdateStreamingPlatformDto streamingPlatformDto)
        {
            var streamingPlatform = _context.StreamingServices.FirstOrDefault(movie => movie.Id == id);

            if (streamingPlatform == null)
                return NotFound();

            _mapper.Map(streamingPlatformDto, streamingPlatform);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes a streaming platform by ID.
        /// </summary>
        /// <param name="id">Identifier of the streaming platform to be deleted.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the streaming platform with the specified ID is not found.</response>
        /// <response code="204">If the delete was successful.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteStreamingPlatformById(int id)
        {
            StreamingPlatform streamingPlatform = _context.StreamingServices.FirstOrDefault(streamingPlatform => streamingPlatform.Id == id);

            if (streamingPlatform == null)
                return NotFound();

            _context.Remove(streamingPlatform);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
