using StreamberryMoviesApi.Models.CustomDataAnnotation;
using System.ComponentModel.DataAnnotations;

namespace StreamberryMoviesApi.Data.Dtos
{
    public class CreateMovieDto
    {
        [Required(ErrorMessage = "Title of the movie is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Genre of the movie is required")]
        public string Genre { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The length of the movie must be greater than 1 minute")]
        public double MovieLenght { get; set; }

        [MonthYear(ErrorMessage = "Invalid format for month/year. Use 'MM/yyyy' format.")]
        public string MonthAndYear { get; set; }
    }
}
