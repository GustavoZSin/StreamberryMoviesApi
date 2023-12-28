using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StreamberryMoviesApi.Data;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private StreamBerryContext _context;
        private IMapper _mapper;
        public RatingController(StreamBerryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new rating to the database.
        /// </summary>
        /// <param name="ratingDto">Object with necessary fields to create a rating.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Returns the newly created rating.</response>
        /// <response code="409">If the user already has a rating for this movie.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult AddRating([FromBody] CreateRatingDto ratingDto)
        {
            if (FindRatingByProperties(ratingDto))
            {
                return Conflict("User already have register a rating to this movie");
            }

            Rating rating = _mapper.Map<Rating>(ratingDto);
            _context.Ratings.Add(rating);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetRatingById), new { id = rating.Id }, rating);
        }

        private bool FindRatingByProperties(CreateRatingDto ratingDto)
        {
            bool ratingExists = _context.Ratings
                .Any(rt => rt.UserId == ratingDto.UserId
                        && rt.MovieId == ratingDto.MovieId);

            if (ratingExists)
                return true;

            return false;
        }


        /// <summary>
        /// Retrieves a list of ratings with optional pagination.
        /// </summary>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of ratings.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ReadRatingDto> GetAllRatings([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadRatingDto>>(_context.Ratings.Skip(skip).Take(take).ToList());
        }

        /// <summary>
        /// Retrieves a specific rating by ID.
        /// </summary>
        /// <param name="id">Identifier of the rating to be retrieved.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the rating with the specified ID is not found.</response>
        /// <response code="200">Returns the requested rating.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetRatingById(int id)
        {
            var rating = _context.Ratings.FirstOrDefault(rating => rating.Id == id);

            if (rating == null)
                return NotFound();

            var ratingDto = _mapper.Map<ReadRatingDto>(rating);

            return Ok(ratingDto);
        }

        /// <summary>
        /// Updates a rating by ID.
        /// </summary>
        /// <param name="id">Identifier of the rating to be updated.</param>
        /// <param name="ratingDto">Object with necessary fields to update a rating.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the rating with the specified ID is not found.</response>
        /// <response code="204">If the update was successful.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateRatingDtoById(int id, [FromBody] UpdateRatingDto ratingDto)
        {
            var rating = _context.Ratings.FirstOrDefault(rating => rating.Id == id);

            if (rating == null)
                return NotFound();

            _mapper.Map(ratingDto, rating);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Updates specific properties of a rating by ID.
        /// </summary>
        /// <param name="id">Identifier of the rating to be updated.</param>
        /// <param name="patch">Object with JSON path configurations to update.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the rating with the specified ID is not found.</response>
        /// <response code="204">If the update was successful.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialRatingPropertiesById(int id, JsonPatchDocument<UpdateRatingDto> patch)
        {
            var rating = _context.Ratings.FirstOrDefault(rating => rating.Id == id);

            if (rating == null)
                return NotFound();

            var ratingToUpdate = _mapper.Map<UpdateRatingDto>(rating);

            patch.ApplyTo(ratingToUpdate, ModelState);

            if (!TryValidateModel(ratingToUpdate))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(ratingToUpdate, rating);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes a rating by ID.
        /// </summary>
        /// <param name="id">Identifier of the rating to be deleted.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the rating with the specified ID is not found.</response>
        /// <response code="204">If the delete was successful.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteRatingById(int id)
        {
            Rating rating = _context.Ratings.FirstOrDefault(rating => rating.Id == id);

            if (rating == null)
                return NotFound();

            _context.Remove(rating);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
