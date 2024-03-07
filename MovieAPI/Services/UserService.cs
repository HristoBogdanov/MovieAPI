using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Data.Models;
using MovieAPI.Services.Interfaces;
using MovieAPI.ViewModels;

namespace MovieAPI.Services
{
    /// <summary>
    /// Service responsible for managing user-related functionalities such as user creation, finding users, retrieving user comments and ratings, user authentication, and generating JWT tokens.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly DataContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;

        /// <summary>
        /// Initializes a new instance of the UserService class.
        /// </summary>
        /// <param name="context">The data context for interacting with the database.</param>
        /// <param name="userManager">The user manager for managing users.</param>
        /// <param name="signInManager">The sign-in manager for user authentication.</param>
        /// <param name="tokenService">The token service for generating JWT tokens.</param>
        public UserService(DataContext context,
                           UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           ITokenService tokenService)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }


        /// <summary>
        /// Creates a new user based on the provided registration data.
        /// </summary>
        /// <param name="register">The registration data provided by the user.</param>
        /// <returns>Returns the newly created user along with a JWT token if successful, otherwise null.</returns>
        public async Task<NewUserDTO> CreateUser(RegisterDTO register)
        {
            var applicationUser = new ApplicationUser()
            {
                UserName = register.Username,
                Email = register.Email
            };
            
            var createdUser = await userManager.CreateAsync(applicationUser, register.Password);

            if(createdUser.Succeeded)
            {
                var role = await userManager.AddToRoleAsync(applicationUser, "User");
                if (role.Succeeded)
                {
                    var newUser = new NewUserDTO()
                    {
                        UserName = applicationUser.UserName,
                        Email = applicationUser.Email,
                        Token = tokenService.CreateToken(applicationUser)
                    };
                    return newUser;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }


        /// <summary>
        /// Finds a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to find.</param>
        /// <returns>Returns the user if found, otherwise null.</returns>
        public async Task<ApplicationUser> FindUser(string username)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == username.ToLower());
            if(user == null)
            {
                return null;
            }
            return user;
        }


        /// <summary>
        /// Retrieves all comments made by the user.
        /// </summary>
        /// <param name="userId">The ID of the user whose comments are to be retrieved.</param>
        /// <returns>Returns a list of comments made by the user, or null if no comments are found.</returns>
        public async Task<List<GetAllUserCommentsDTO>> GetAllUserComments(string userId)
        {
            var comments = await context.MoviesComments.Include(c => c.Comment).Where(c => c.Comment.UserId == userId).Include(c => c.Movie).Select(c => new GetAllUserCommentsDTO
            {
                Movie = c.Movie.Name,
                Comment = c.Comment.Description,
            }).ToListAsync();

            if(comments.Count() == 0) 
            {
                return null;
            }

            return comments;
        }


        /// <summary>
        /// Retrieves all ratings made by the user.
        /// </summary>
        /// <param name="userId">The ID of the user whose ratings are to be retrieved.</param>
        /// <returns>Returns a list of ratings made by the user, or null if no ratings are found.</returns>
        public async Task<List<GetAllUserRatingsDTO>> GetAllUserRatings(string userId)
        {
            var ratings = await context.MoviesRatings.Include(r => r.Rating).Where(r => r.Rating.UserId == userId).Include(r => r.Movie).Select(r => new GetAllUserRatingsDTO
            {
                Movie = r.Movie.Name,
                Rating = r.Rating.RatingScore,
            }).ToListAsync();

            if(ratings.Count() == 0)
            {
                return null;
            }

            return ratings;
        }


        /// <summary>
        /// Retrieves information about the logged-in user along with a JWT token.
        /// </summary>
        /// <param name="user">The user whose information is to be retrieved.</param>
        /// <returns>Returns information about the logged-in user along with a JWT token.</returns>
        public NewUserDTO GetLoggedUser(ApplicationUser user)
        {
            var loggedUser = new NewUserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            };
            return loggedUser;
        }


        /// <summary>
        /// Attempts to sign in a user with the provided credentials.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password provided by the user.</param>
        /// <returns>Returns the result of the sign-in attempt.</returns>
        public async Task<SignInResult> TrySignIn(ApplicationUser user, string password)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            return result;
        }
    }
}
