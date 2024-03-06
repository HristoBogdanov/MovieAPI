using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Services.Interfaces;
using MovieAPI.ViewModels;

namespace MovieAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext context;
        public UserService(DataContext context)
        {
            this.context = context;
        }

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
    }
}
