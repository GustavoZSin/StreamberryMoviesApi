using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StreamberryMoviesApi.Models
{
    public class Rating
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Field rate is required")]
        [Range(1,5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        public int Rate { get; set; }
        public string Commentary { get; set; }
        public string? UserId { get; set; }
        public virtual User User { get; set; }
        public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
    }
}