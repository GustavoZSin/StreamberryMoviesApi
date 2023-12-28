using System.ComponentModel.DataAnnotations;

namespace StreamberryMoviesApi.Models
{
    public class StreamingPlatform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Streaming service name is required")]
        public string Name { get; set; }

        public virtual ICollection<MovieStreamingAssociation> ServicesOfStreaming { get; set; }
    }
}
