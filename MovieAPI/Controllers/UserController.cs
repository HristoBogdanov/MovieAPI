using Microsoft.AspNetCore.Mvc;
using MovieAPI.Services.Interfaces;
using MovieAPI.ViewModels;
using System.Security.Claims;

namespace MovieAPI.Controllers
{
    /// <summary>
    /// Controller for managing user-related actions such as login, registration, and retrieving user comments and ratings.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        /// <param name="userService">The user service for interacting with user data.</param>
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }


        /// <summary>
        /// Endpoint for user login.
        /// </summary>
        /// <param name="login">The login credentials provided by the user.</param>
        /// <returns>Returns the result of the login attempt.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userService.FindUser(login.UserName);

            if(user == null)
            {
                return Unauthorized("Invalid username!");
            }

            var result = await userService.TrySignIn(user, login.Password);
            
            if (!result.Succeeded)
            {
                return Unauthorized("Username/Password not found!");
            }

            var loggedUser = userService.GetLoggedUser(user);
            return Ok(loggedUser);
        }


        /// <summary>
        /// Endpoint for user registration.
        /// </summary>
        /// <param name="register">The registration data provided by the user.</param>
        /// <returns>Returns the result of the registration attempt.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdUser = await userService.CreateUser(register);

                if (createdUser != null)
                {
                    return Ok(createdUser);
                }
                else
                {
                    return BadRequest("Something went wrong!");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong!");
            }
        }


        /// <summary>
        /// Endpoint for retrieving all comments made by the user.
        /// </summary>
        /// <returns>Returns the comments made by the user.</returns>
        [HttpGet("get-all-comments")]
        public async Task<IActionResult> GetAllUserComments()
        {
            var comments = await userService.GetAllUserComments(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(comments == null)
            {
                return NotFound();
            }

            return Ok(comments);
        }


        /// <summary>
        /// Endpoint for retrieving all ratings made by the user.
        /// </summary>
        /// <returns>Returns the ratings made by the user.</returns>
        [HttpGet("get-all-ratings")]
        public async Task<IActionResult> GetAllUserRatings()
        {
            var ratings = await userService.GetAllUserRatings(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ratings == null)
            {
                return NotFound();
            };

            return Ok(ratings);
        }

    }
}
