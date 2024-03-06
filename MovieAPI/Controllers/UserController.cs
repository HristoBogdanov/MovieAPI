using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Services.Interfaces;
using System.Security.Claims;

namespace MovieAPI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

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
