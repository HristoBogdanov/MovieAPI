using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Services.Interfaces;
using MovieAPI.ViewModels;
using System.Security.Claims;

namespace MovieAPI.Controllers
{

    /// <summary>
    /// Controller class responsible for handling movie-related HTTP requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService movieService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController"/> class.
        /// </summary>
        /// <param name="movieService">The movie service instance.</param>
        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }


        /// <summary>
        /// Retrieves all movies from the database.
        /// </summary>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [Authorize]
        [HttpGet("get-all-movies")]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await movieService.GetAllMovies();
            if(movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }


        /// <summary>
        /// Retrieves a movie by its ID from the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("get-movie/{movieId}")]
        public async Task<IActionResult> GetMovie([FromRoute]int movieId)
        {
            var movie = await movieService.GetMovie(movieId);

            if(movie == null) 
            { 
                return NotFound();
            }

            return Ok(movie);
        }


        /// <summary>
        /// Adds a movie to the database.
        /// </summary>
        /// <param name="movie">The movie data to add.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpPost("create-movie")]
        public async Task<IActionResult> AddMovie([FromForm]AddMovieDTO movie)
        {
            if(ModelState.IsValid) 
            {
                var addedMovie = await movieService.AddMovie(movie);
                return Ok(addedMovie);
            }

            return BadRequest();
        }


        /// <summary>
        /// Updates a movie in the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie to update.</param>
        /// <param name="updates">The updated movie data.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpPut("update-movie/{movieId}")]
        public async Task<IActionResult> UpdateMovie([FromRoute]int movieId, [FromForm]UpdateMovieDTO updates)
        {
            if(ModelState.IsValid) 
            { 
                var updatedMovie = await movieService.UpdateMovie(movieId, updates);
                if(updatedMovie == null)
                {
                    return BadRequest();
                }
                return Ok(updatedMovie);
            }

            return BadRequest();
        }


        /// <summary>
        /// Deletes a movie from the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie to delete.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpDelete("delete-movie/{movieId}")]
        public async Task<IActionResult> DeleteMovie([FromRoute]int movieId)
        {
            var successful = await movieService.DeleteMovie(movieId);
            if(successful) 
            { 
                return Ok();
            }
            return BadRequest();
        }


        /// <summary>
        /// Retrieves all comments of a movie from the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("get-all-movie-comments/{movieId}")]
        public async Task<IActionResult> GetAllMovieComments([FromRoute] int movieId)
        {
            var movie = await movieService.GetMovie(movieId);
            
            if (movie == null)
            {
                return NotFound();
            }

            var comments = await movieService.GetMovieComments(movieId);
            return Ok(comments);
        }


        /// <summary>
        /// Adds a comment to a movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <param name="commentToAdd">The comment data to add.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize]
        [HttpPost("write-comment/{movieId}")]
        public async Task<IActionResult> CommentMovie(int movieId, AddCommentDTO commentToAdd)
        {
            var movie = await movieService.GetMovie(movieId);

            if (movie == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var addedComment = await movieService.AddComment(commentToAdd, User.FindFirstValue(ClaimTypes.NameIdentifier), movieId);
                if(addedComment == null) 
                { 
                    return BadRequest();
                }
                return Ok(addedComment);
            }

            return null;
        }


        /// <summary>
        /// Rates a movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <param name="rating">The rating value.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize]
        [HttpPost("rate-movie/{movieId}")]
        public async Task<IActionResult> RateMovie(int movieId, int rating)
        {
            var movie = await movieService.GetMovie(movieId);

            if (movie == null)
            {
                return NotFound();
            }

            if(rating >= 1 || rating <= 10) 
            {
                var res = await movieService.AddRating(movieId, rating, User.FindFirstValue(ClaimTypes.NameIdentifier));
                
                if(res == false)
                {
                    return BadRequest();
                }

                return Ok();
            }

            return null;
        }


        /// <summary>
        /// Updates a comment in the database.
        /// </summary>
        /// <param name="commentId">The ID of the comment to update.</param>
        /// <param name="updates">The updated comment data.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize]
        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateComment(int commentId, UpdateCommentDTO updates)
        {
            if (ModelState.IsValid)
            {
                var updatedComment = await movieService.UpdateComment(commentId, User.FindFirstValue(ClaimTypes.NameIdentifier), updates);
                if (updatedComment == null)
                {
                    return BadRequest();
                }
                return Ok(updatedComment);
            }

            return BadRequest();
        }


        /// <summary>
        /// Deletes a comment from the database.
        /// </summary>
        /// <param name="commentid">The ID of the comment to delete.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize]
        [HttpDelete("delete-comment")]
        public async Task<IActionResult> DeleteComment(int commentid)
        {
            var successful = await movieService.DeleteComment(commentid);
            if (successful)
            {
                return Ok();
            }
            return BadRequest();
        }


        /// <summary>
        /// Updates a review in the database.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to update.</param>
        /// <param name="rating">The updated rating value.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        //[Authorize]
        [HttpPut("update-rating")]
        public async Task<IActionResult> UpdateReview(int ratingId, int rating)
        {
            if (ModelState.IsValid)
            {
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var updatedRating = await movieService.UpdateRating(ratingId, User.FindFirstValue(ClaimTypes.NameIdentifier), rating);
                if (updatedRating == null)
                {
                    return BadRequest();
                }
                return Ok(updatedRating);
            }

            return BadRequest();
        }


        /// <summary>
        /// Searches for movies by name in the database.
        /// </summary>
        /// <param name="movieName">The name of the movie to search for.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("search-movie-by-name")]
        public async Task<IActionResult> SearchMovieByName(string movieName)
        {
            var movies = await movieService.GetMovieByName(movieName);

            if (movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }


        /// <summary>
        /// Retrieves movies ordered by release date in ascending order.
        /// </summary>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("order-movie-by-release-date-asc")]
        public async Task<IActionResult> OrderMovieByReleaseDateAsc()
        {
            var movie = await movieService.GetOrderedMovieByReleaseDateAsc();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }


        /// <summary>
        /// Retrieves movies ordered by release date in descending order.
        /// </summary>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("order-movie-by-release-date-desc")]
        public async Task<IActionResult> OrderMovieByReleaseDateDesc()
        {
            var movie = await movieService.GetOrderedMovieByReleaseDateDesc();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }


        /// <summary>
        /// Retrieves movies ordered by name in ascending order.
        /// </summary>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("order-movie-by-name-asc")]
        public async Task<IActionResult> OrderMovieByNameAsc()
        {
            var movie = await movieService.GetOrderedMovieByNameAsc();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }


        /// <summary>
        /// Retrieves movies ordered by name in descending order.
        /// </summary>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("order-movie-by-name-desc")]
        public async Task<IActionResult> OrderMovieByNameDesc()
        {
            var movie = await movieService.GetOrderedMovieByNameDesc();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }


        /// <summary>
        /// Deletes an image associated with a movie from the database.
        /// </summary>
        /// <param name="imageId">The ID of the image to delete.</param>
        /// <param name="movieId">The ID of the movie associated with the image.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpDelete("delete-image")]
        public async Task<IActionResult> DeleteImageById(int imageId, int movieId)
        {
            var successful = await movieService.DeleteImage(imageId, movieId);
            if (successful)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
