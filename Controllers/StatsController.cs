using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamberryMoviesApi.Data;
using StreamberryMoviesApi.Data.Dtos;
using System.Globalization;

namespace StreamberryMoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class StatsController : ControllerBase
    {
        private StreamBerryContext _context;
        private IMapper _mapper;
        public StatsController(StreamBerryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the number of streaming services available for a specific movie.
        /// </summary>
        /// <param name="title">Title of the movie.</param>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the number of streaming services available for the movie.</response>
        /// <response code="404">If the movie with the specified title is not found.</response>
        [HttpGet("streams-by-movie")]
        public IActionResult GetNumberOfStreamsByMovie([FromQuery] string title, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var movie = _context.Movies
                .Include(m => m.ServicesOfStreaming)
                .FirstOrDefault(m => m.Title == title);

            if (movie == null)
            {
                return NotFound($"Movie with title '{title}' not found.");
            }

            int numberOfStreamsTheMovieIsAvailable = movie.ServicesOfStreaming.Count;

            return Ok(new { Title = movie.Title, NumberOfStreamsTheMovieIsAvailable = numberOfStreamsTheMovieIsAvailable });
        }

        /// <summary>
        /// Gets the average rating of all movies.
        /// </summary>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of movies with their average ratings.</response>
        [HttpGet("average-rating-of-all-movies")]
        public IActionResult GetAverageRatingOfAllMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var movies = _context.Movies
                .Skip(skip)
                .Take(take)
                .ToList();

            var averageRatings = movies.Select(movie => new
            {
                MovieId = movie.Id,
                Title = movie.Title,
                AverageRating = _context.Ratings
                    .Where(rating => rating.MovieId == movie.Id)
                    .Select(rating => rating.Rate)
                    .DefaultIfEmpty()
                    .Average()
            })
            .ToList();

            return Ok(averageRatings);
        }

        /// <summary>
        /// Gets the number of movies released per year.
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of years with the sum of movies released each year.</response>
        [HttpGet("movies-per-year")]
        public IActionResult GetNumberOfMoviesPerYear()
        {
            var moviesPerYear = _context.Movies
                .AsEnumerable()
                .Select(movie => new
                {
                    Movie = movie,
                    Year = GetYear(movie.MonthAndYear),
                })
                .GroupBy(item => item.Year)
                .Select(group => new
                {
                    SumMoviesByYear = group.Count(),
                    Year = group.Key
                })
                .ToList();

            return Ok(moviesPerYear);
        }

        /// <summary>
        /// Gets movies based on their rating, commentary, or both.
        /// </summary>
        /// <param name="rating">The rating value to filter movies.</param>
        /// <param name="comment">The commentary text to filter movies.</param>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of movies matching the specified criteria.</response>
        [HttpGet("movies-per-rate-and-or-commentary")]
        public IActionResult GetMoviesPerRateAndOrCommentary([FromQuery] int? rating = null, [FromQuery] string comment = "", [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var query = _context.Movies.AsQueryable();

            if (rating.HasValue)
            {
                var ratingValue = rating.Value;
                query = query.Where(movie => _context.Ratings.Any(rate => rate.MovieId == movie.Id && rate.Rate == ratingValue));
            }

            if (!string.IsNullOrWhiteSpace(comment))
            {
                var commentValue = comment;
                query = query.Where(movie => _context.Ratings.Any(c => c.MovieId == movie.Id && c.Commentary == commentValue));
            }

            var movies = query
                .OrderBy(movie => movie.Id)
                .Skip(skip)
                .Take(take)
                .ToList()
                .Select(movie => new
                {
                    Movie = _mapper.Map<ReadMovieDto>(movie),
                    Ratings = _mapper.Map<List<ReadRatingDto>>(_context.Ratings
                        .Where(r => r.MovieId == movie.Id)
                        .ToList())
                })
                .ToList();

            return Ok(movies);
        }

        /// <summary>
        /// Gets the average rating per genre and per release year.
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of genres with their average ratings for each release year.</response>
        [HttpGet("average-rating-per-genre-and-per-year")]
        public IActionResult GetAverageRatingPerGenreAndPerYear()
        {
            var averageRatingsByGenreAndReleaseYear = _context.Movies
                .AsEnumerable()  // Força a avaliação no lado do cliente
                .GroupBy(movie => new { movie.Genre, ReleaseYear = GetYear(movie.MonthAndYear) })
                .Select(group => new
                {
                    Genre = group.Key.Genre,
                    ReleaseYear = group.Key.ReleaseYear,
                    AverageRating = group
                        .SelectMany(movie => movie.Ratings)
                        .DefaultIfEmpty()
                        .Average(rating => rating != null ? rating.Rate : 0)
                })
                .ToList();

            return Ok(averageRatingsByGenreAndReleaseYear);
        }

        private string GetYear(string monthAndYear)
        {
            if (DateTime.TryParseExact(monthAndYear, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date.Year.ToString();
            }

            return "";
        }
    }
}