using System.ComponentModel.DataAnnotations;

namespace StreamberryMoviesApi.Data.Dtos
{
    public class UpdateRatingDto
    {
        [Required(ErrorMessage = "Field rate is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        public int Rate { get; set; }
        public string Commentary { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
    }
}
