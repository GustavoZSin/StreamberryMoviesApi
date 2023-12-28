using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StreamberryMoviesApi.Controllers
{
    public class AccessController : ControllerBase
    {
        /// <summary>
        /// Validates if the user is logged in according to the JWT token.
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="404">If there is an error validating the JWT token.</response>
        /// <response code="200">User logged in successfully.</response>
        [HttpGet]
        [Authorize(Policy = "RequireAuthenticatedUser")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok("Access allowed");
        }
    }
}
