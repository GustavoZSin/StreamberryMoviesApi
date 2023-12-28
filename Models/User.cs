using Microsoft.AspNetCore.Identity;

namespace StreamberryMoviesApi.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Rating> Ratings { get; set; }
        public User() : base() { }
    }
}
