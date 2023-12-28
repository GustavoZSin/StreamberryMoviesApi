using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;
using StreamberryMoviesApi.Services;

namespace StreamberryMoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private UserService _userService;
        public UserController(UserManager<User> userManager, UserService registerService)
        {
            _userService = registerService;
            _userManager = userManager;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="userDto">Object with necessary fields to create a user.</param>
        /// <returns>IActionResult</returns>
        /// <response code="400">If an error occurs during the registration process.</response>
        /// <response code="200">User successfully registered.</response>
        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto userDto)
        {
            await _userService.RegisterAsync(userDto);
            return Ok("The user has been registered successfully");
        }

        /// <summary>
        /// Logs in to the API and returns a JWT token.
        /// </summary>
        /// <param name="loginDto">Object with necessary fields to realize login.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">User successfully logged.</response>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUserDto loginDto)
        {
            var token = await _userService.LoginAsync(loginDto);
            return Ok(token);
        }

        /// <summary>
        /// Retrieves a list of users with optional pagination.
        /// </summary>
        /// <param name="skip">Integer that informs the pagination configuration.</param>
        /// <param name="take">Integer that informs how many objects will be returned.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns the list of users.</response>
        [HttpGet("GetAll")]
        public IActionResult GetAllUsers([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var users = _userManager.Users.Skip(skip).Take(take).ToList();

            var userDtos = users.Select(user => new ReadUserDto
            {
                UserName = user.UserName,
                Id = user.Id
            }).ToList();

            return Ok(userDtos);
        }

        /// <summary>
        /// Retrieves a specific user by ID.
        /// </summary>
        /// <param name="id">Identifier of the user to be retrieved.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="200">Returns the requested user.</response>
        [HttpGet("GetById")]
        public IActionResult GetUserById(string id)
        {
            var user = _userManager.Users.FirstOrDefault(us => us.Id == id);

            if (user == null)
                return NotFound();

            var userDto = new ReadUserDto { UserName = user.UserName, Id = user.Id };

            return Ok(userDto);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">Identifier of the user to be deleted.</param>
        /// <returns>IActionResult</returns>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="204">If the delete was successful.</response>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return NoContent();

            return BadRequest(result.Errors);
        }
    }
}