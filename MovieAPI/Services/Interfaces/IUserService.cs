using MovieAPI.ViewModels;

namespace MovieAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<GetAllUserCommentsDTO>> GetAllUserComments(string userId);
        Task<List<GetAllUserRatingsDTO>> GetAllUserRatings(string userId);
    }
}
