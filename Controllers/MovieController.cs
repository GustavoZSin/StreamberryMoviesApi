using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StreamberryMoviesApi.Data;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private StreamBerryContext _context;
        private IMapper _mapper;
        public MovieController(StreamBerryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new movie in the database.
        /// </summary>
        /// <param name="movieDto">Object with necessary fields to create a movie.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Returns the newly created movie.</response>
        /// <response code="409">If the movie already exists in the database.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult AddMovie([FromBody] CreateMovieDto movieDto)
        {
            if (FindMovieByTitle(movieDto.Title))
            {
                return Conflict("Movie already exists");
            }

            Movie movie = _mapper.Map<Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }
        private bool FindMovieByTitle(string title)
        {
            bool movieExists = _context.Movies.Any(movie => movie.Title == title);

            if (movieExists)
                return true;

            return false;
        }

        /// <summary>
        /// Retrieves a specific movie by ID.
        /// </summary>
        /// <param name="id">Identifier of the movie to be retrieved.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the movie specified is not found.</response>
        /// <response code="200">Returns the requested movie.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovieById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);

            if (movie == null)
                return NotFound();

            var movieDto = _mapper.Map<ReadMovieDto>(movie);

            return Ok(movieDto);
        }

        /// <summary>
        /// Retrieves a list of movies with optional pagination.
        /// </summary>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of movies.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ReadMovieDto> GetAllMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadMovieDto>>(_context.Movies.Skip(skip).Take(take).ToList());
        }

        /// <summary>
        /// Retrieves a list of movies by genres with optional pagination.
        /// </summary>
        /// <param name="movieGenre">Name of genre that will be searched.</param>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of movies.</response>
        [HttpGet("genre/{movieGenre}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ReadMovieDto> GetAllMoviesByGenre(string movieGenre, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadMovieDto>>(_context.Movies.Where(genre => genre.Genre == movieGenre).Skip(skip).Take(take).ToList());
        }

        /// <summary>
        /// Updates a movie by ID.
        /// </summary>
        /// <param name="id">Identifier of the movie to be updated.</param>
        /// <param name="movieDto">Object with necessary fields to update a movie.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the movie with the specified ID is not found.</response>
        /// <response code="204">If the update was successful.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateMovieById(int id, [FromBody] UpdateMovieDto movieDto)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);

            if (movie == null)
                return NotFound();

            _mapper.Map(movieDto, movie);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Updates specific properties of a movie by ID.
        /// </summary>
        /// <param name="id">Identifier of the movie to be updated.</param>
        /// <param name="patch">Object with JSON path configurations to update.</param>
        /// <returns>IActionResult</returns>
        /// <response code="400">If there is an error in validating the movie.</response>
        /// <response code="404">If the movie with the specified ID is not found.</response>
        /// <response code="204">If the update was successful.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialMoviePropertiesById(int id, JsonPatchDocument<UpdateMovieDto> patch)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);

            if (movie == null)
                return NotFound();

            var movieToUpdate = _mapper.Map<UpdateMovieDto>(movie);

            patch.ApplyTo(movieToUpdate, ModelState);

            if (!TryValidateModel(movieToUpdate))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(movieToUpdate, movie);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes a movie by ID.
        /// </summary>
        /// <param name="id">Identifier of the movie to be deleted.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the movie with the specified ID is not found.</response>
        /// <response code="204">If the delete was successful.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteMovieById(int id)
        {
            Movie movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);

            if (movie == null)
                return NotFound();

            _context.Remove(movie);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
