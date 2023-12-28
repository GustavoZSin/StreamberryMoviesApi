using System.ComponentModel.DataAnnotations;

namespace StreamberryMoviesApi.Data.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string RePassword { get; set; }
    }
}