using System.ComponentModel.DataAnnotations;

namespace StreamberryMoviesApi.Data.Dtos
{
    public class CreateStreamingPlatformDto
    {
        [Required(ErrorMessage = "Streaming service name is required")]
        public string Name { get; set; }
    }
}
