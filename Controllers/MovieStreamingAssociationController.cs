using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StreamberryMoviesApi.Data;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieStreamingAssociationController : ControllerBase
    {
        private StreamBerryContext _context;
        private IMapper _mapper;
        public MovieStreamingAssociationController(StreamBerryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Adds a new movie streaming association to the database.
        /// </summary>
        /// <param name="dto">Object with necessary fields to create a movie streaming association.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Returns the newly created movie streaming association.</response>
        /// <response code="409">If the associtaion between movie-streaming already exists in the database.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult AddMovieStreamingAssociation(CreateMovieStreamingAssociationDto dto)
        {
            if (FindAssociationByIds(dto))
            {
                return Conflict("Association between this movie and streaming service already exists");
            }

            MovieStreamingAssociation movieStreamingAssociation = _mapper.Map<MovieStreamingAssociation>(dto);
            _context.Movies_StreamingServices.Add(movieStreamingAssociation);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMovieStreamingAssociationById), new
            {
                movieId = movieStreamingAssociation.MovieId,
                streamingServiceId = movieStreamingAssociation.StreamingServiceId
            }, movieStreamingAssociation);
        }

        private bool FindAssociationByIds(CreateMovieStreamingAssociationDto dto)
        {
            bool linkExists = _context.Movies_StreamingServices
                .Any(link => link.MovieId == dto.MovieId && link.StreamingServiceId == dto.StreamingServiceId);

            if (linkExists)
                return true;

            return false;
        }

        /// <summary>
        /// Retrieves a list of movie streaming associations with optional pagination.
        /// </summary>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of movie streaming associations.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ReadMovieStreamingAssociationDto> GetAllMovieStreamingAssociations([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadMovieStreamingAssociationDto>>(_context.Movies_StreamingServices.Skip(skip).Take(take).ToList());
        }

        /// <summary>
        /// Retrieves a specific movie streaming association by ID.
        /// </summary>
        /// <param name="movieId">Identifier of the movie in the association.</param>
        /// <param name="streamingServiceId">Identifier of the streaming service in the association.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the movie streaming association with the specified IDs is not found.</response>
        /// <response code="200">Returns the requested movie streaming association.</response>
        [HttpGet("{movieId}/{streamingServiceId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovieStreamingAssociationById(int movieId, int streamingServiceId)
        {
            MovieStreamingAssociation movieStreamingAssociation = _context.Movies_StreamingServices
                .FirstOrDefault(movieStreamingAssociation => movieStreamingAssociation.MovieId == movieId
                                && movieStreamingAssociation.StreamingServiceId == streamingServiceId);

            if (movieStreamingAssociation != null)
            {
                ReadMovieStreamingAssociationDto movieStreamingAssociationDto = _mapper.Map<ReadMovieStreamingAssociationDto>(movieStreamingAssociation);

                return Ok(movieStreamingAssociationDto);
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes a movie streaming association by ID.
        /// </summary>
        /// <param name="movieId">Identifier of the movie in the association.</param>
        /// <param name="streamingServiceId">Identifier of the streaming service in the association.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the movie streaming association with the specified IDs is not found.</response>
        /// <response code="204">If the delete was successful.</response>
        [HttpDelete("{movieId}/{streamingServiceId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteMovieStreamingAssociationByIds(int movieId, int streamingServiceId)
        {
            MovieStreamingAssociation msLink = _context.Movies_StreamingServices.FirstOrDefault(link => link.MovieId == movieId && link.StreamingServiceId == streamingServiceId);

            if (msLink == null)
                return NotFound();

            _context.Remove(msLink);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
