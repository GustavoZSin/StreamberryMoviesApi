using System.ComponentModel.DataAnnotations;

namespace StreamberryMoviesApi.Data.Dtos
{
    public class UpdateStreamingPlatformDto
    {
        [Required(ErrorMessage = "Streaming service name is required")]
        public string Name { get; set; }
    }
}
