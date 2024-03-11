using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Data;
using MovieAPI.Data.Models;
using MovieAPI.Services.Interfaces;
using MovieAPI.ViewModels;

namespace MovieAPI.Services
{
    /// <summary>
    /// Service class responsible for handling movie-related operations.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly DataContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieService"/> class.
        /// </summary>
        /// <param name="context">The DataContext.</param>
        /// <param name="webHostEnvironment">The IWebHostEnvironment.</param>
        public MovieService(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }


        /// <summary>
        /// Adds a comment to a movie.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <param name="userId">The ID of the user adding the comment.</param>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>The added comment.</returns>
        public async Task<Comment> AddComment(AddCommentDTO comment, string userId, int movieId)
        {
            var existingComment = await context.Comments
                .FirstOrDefaultAsync(x => x.Description == comment.Description && x.UserId == userId);

            if (existingComment != null)
            {
                return null;
            }

            Comment commentToAdd = new Comment()
            {
                Description = comment.Description,
                UserId = userId,
            };

            await context.Comments.AddAsync(commentToAdd);
            await context.SaveChangesAsync();

            MoviesComments movieComment = new MoviesComments()
            {
                CommentId = commentToAdd.Id,
                MovieId = movieId,
            };

            await context.MoviesComments.AddAsync(movieComment);
            await context.SaveChangesAsync();

            return commentToAdd;
        }


        /// <summary>
        /// Adds a movie to the database.
        /// </summary>
        /// <param name="movie">The movie to add.</param>
        /// <returns>The added movie.</returns>
        public async Task<Movie> AddMovie(AddMovieDTO movie)
        {
            var movies = await context.Movies.ToListAsync();
            if(movies.Any(m => m.Name == movie.Name))
            {
                return null;
            } 
            
            Movie movieToAdd = new Movie()
            {
                Name = movie.Name,
                Description = movie.Description,
                DirectorName = movie.DirectorName,
                ReleaseDate = DateTime.Parse(movie.ReleaseDate),
                Length = movie.Length,
            };
            await context.Movies.AddAsync(movieToAdd);
            await context.SaveChangesAsync();

            var files = movie.files;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    var isValidImage = IsImageValid(file);

                    if (isValidImage)
                    {
                        var uniqueFileName = GetUniqueFileName(file);
                        var filePath = SaveImageToFile(file, uniqueFileName);

                        var image = await SaveImageToDatabase(filePath, context);

                        await SaveMovieImageToDatabase(movieToAdd.Id, image.Id, context);
                    }
                }
            }

            return movieToAdd;
        }


        /// <summary>
        /// Adds a rating to a movie.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <param name="rating">The rating to add.</param>
        /// <param name="userId">The ID of the user adding the rating.</param>
        /// <returns>True if the rating is added successfully; otherwise, false.</returns>
        public async Task<bool> AddRating(int movieId, int rating, string userId)
        {
            var movieRatings = await context.MoviesRatings.Include(mr => mr.Rating).ToListAsync();
            var newRating = new Rating()
            {
                UserId = userId,
                RatingScore = rating,
            };
            await context.Ratings.AddAsync(newRating);

            if (movieRatings.Any(mr => mr.MovieId == movieId && mr.Rating.UserId == userId))
            {
                return false;
            }
            
            var movieRating = new MoviesRatings()
            {
                MovieId = movieId,
                RatingId = newRating.Id
            };
            await context.MoviesRatings.AddAsync(movieRating);
            await context.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// Deletes a comment from the database.
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete.</param>
        /// <returns>True if the comment is deleted successfully; otherwise, false.</returns>
        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                context.Remove(comment);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Deletes a movie from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>True if the movie is deleted successfully; otherwise, false.</returns>
        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await context.Movies.FindAsync(id);
            var movieImages = await context.MoviesImages.Where(x => x.MovieId == id).ToListAsync();
            if (movie != null)
            {
                if (movieImages.Any())
                {
                    context.MoviesImages.RemoveRange(movieImages);
                }

                context.Remove(movie);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Retrieves all movies from the database.
        /// </summary>
        /// <returns>A list of GetAllMoviesDTO objects representing all movies in the database.</returns>
        public async Task<List<GetAllMoviesDTO>> GetAllMovies()
        {
            var movies = await context.Movies.Select(movie => new GetAllMoviesDTO
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                DirectorName = movie.DirectorName,
                ReleaseDate = movie.ReleaseDate,
                Length = movie.Length,
                Images = context.MoviesImages.Where(mi => mi.MovieId == movie.Id).Include(mi => mi.Image).Select(mi => mi.Image.Path).ToList(),
            }).ToListAsync();

            if(movies.Count == 0) 
            {
                return null;
            }

            return movies;
        }


        /// <summary>
        /// Retrieves a movie by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>A GetAllMoviesDTO object representing the retrieved movie.</returns>
        public async Task<GetAllMoviesDTO> GetMovie(int id)
        {
            var movie = await context.Movies.Where(m => m.Id == id).Select(movie => new GetAllMoviesDTO
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                DirectorName = movie.DirectorName,
                ReleaseDate = movie.ReleaseDate,
                Length = movie.Length,
                Images = context.MoviesImages.Where(mi => mi.MovieId == id).Include(mi => mi.Image).Select(mi => mi.Image.Path).ToList(),
            }).FirstOrDefaultAsync();

            if(movie == null) 
            {
                return null;
            }

            return movie;
        }


        /// <summary>
        /// Retrieves movies by name from the database.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>A list of GetAllMoviesDTO objects representing the retrieved movies.</returns>
        public async Task<List<GetAllMoviesDTO>> GetMovieByName(string name)
        {
            var movies = await context.Movies.Where(m => m.Name.ToLower().Contains(name.ToLower())).Select(m => new GetAllMoviesDTO
            {
                Id=m.Id,
                Name = m.Name,
                Description = m.Description,
                DirectorName = m.DirectorName,
                ReleaseDate = m.ReleaseDate,
                Length = m.Length,
                Images = context.MoviesImages.Where(mi => mi.MovieId == m.Id).Include(mi => mi.Image).Select(mi => mi.Image.Path).ToList(),
            }).ToListAsync();

            if(movies.Count == 0)
            {
                return null;
            }

            return movies;
        }


        /// <summary>
        /// Retrieves comments of a movie from the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>A list of GetAllMovieCommentsDTO objects representing the comments of the movie.</returns>
        public async Task<List<GetAllMovieCommentsDTO>> GetMovieComments(int movieId)
        {
            var movie = await context.Movies.FindAsync(movieId);
            var comments = await context.MoviesComments.Where(x => x.MovieId == movieId).Select(x =>
            new GetAllMovieCommentsDTO 
            {
                User = x.Comment.User.UserName,
                Description = x.Comment.Description,
            }).ToListAsync();

            if(comments.Count() == 0) 
            {
                return null;
            }

            return comments;
        }


        /// <summary>
        /// Retrieves movies ordered by release date in ascending order.
        /// </summary>
        /// <returns>A list of GetAllMoviesDTO objects representing the retrieved movies.</returns>
        public async Task<List<GetAllMoviesDTO>> GetOrderedMovieByReleaseDateAsc()
        {
            var movies = await context.Movies.Select(m => new GetAllMoviesDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                DirectorName = m.DirectorName,
                Length = m.Length,
                Images = context.MoviesImages.Where(mi => mi.MovieId ==m.Id).Include(mi => mi.Image).Select(mi => mi.Image.Path).ToList(),
            }).OrderBy(m => m.ReleaseDate).ToListAsync();

            if(movies.Count == 0) 
            {
                return null;
            }

            return movies;
        }


        /// <summary>
        /// Retrieves movies ordered by release date in descending order.
        /// </summary>
        /// <returns>A list of GetAllMoviesDTO objects representing the retrieved movies.</returns>
        public async Task<List<GetAllMoviesDTO>> GetOrderedMovieByReleaseDateDesc()
        {
            var movies = await context.Movies.Select(m => new GetAllMoviesDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                DirectorName = m.DirectorName,
                Length = m.Length,
                Images = context.MoviesImages.Where(mi => mi.MovieId == m.Id).Include(mi => mi.Image).Select(mi => mi.Image.Path).ToList(),
            }).OrderByDescending(m => m.ReleaseDate).ToListAsync();

            if (movies.Count == 0)
            {
                return null;
            }

            return movies;
        }


        /// <summary>
        /// Retrieves movies ordered by name in ascending order.
        /// </summary>
        /// <returns>A list of GetAllMoviesDTO objects representing the retrieved movies.</returns>
        public async Task<List<GetAllMoviesDTO>> GetOrderedMovieByNameAsc()
        {
            var movies = await context.Movies.Select(m => new GetAllMoviesDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                DirectorName = m.DirectorName,
                Length = m.Length,
                Images = context.MoviesImages.Where(mi => mi.MovieId == m.Id).Include(mi => mi.Image).Select(mi => mi.Image.Path).ToList(),
            }).OrderBy(m => m.Name).ToListAsync();

            if (movies.Count == 0)
            {
                return null;
            }

            return movies;
        }


        /// <summary>
        /// Retrieves movies ordered by name in descending order.
        /// </summary>
        /// <returns>A list of GetAllMoviesDTO objects representing the retrieved movies.</returns>
        public async Task<List<GetAllMoviesDTO>> GetOrderedMovieByNameDesc()
        {
            var movies = await context.Movies.Select(m => new GetAllMoviesDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                DirectorName = m.DirectorName,
                Length = m.Length,
                Images = context.MoviesImages.Where(mi => mi.MovieId == m.Id).Include(mi => mi.Image).Select(mi => mi.Image.Path).ToList(),
            }).OrderByDescending(m => m.Name).ToListAsync();

            if (movies.Count == 0)
            {
                return null;
            }

            return movies;
        }


        /// <summary>
        /// Updates a comment in the database.
        /// </summary>
        /// <param name="commentId">The ID of the comment to update.</param>
        /// <param name="userId">The ID of the user updating the comment.</param>
        /// <param name="updates">The updated comment data.</param>
        /// <returns>The updated comment.</returns>
        public async Task<Comment> UpdateComment(int commentId, string userId, UpdateCommentDTO updates)
        {
            var commentToUpdate = await context.Comments.Where(c => c.Id == commentId && c.UserId == userId).FirstOrDefaultAsync();
            
            if(commentToUpdate == null) 
            {
                return null;
            }

            commentToUpdate.Description = updates.Description;

            await context.SaveChangesAsync();

            return commentToUpdate;
        }


        /// <summary>
        /// Updates a movie in the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie to update.</param>
        /// <param name="updates">The updated movie data.</param>
        /// <returns>The updated movie.</returns>
        public async Task<Movie> UpdateMovie(int movieId, UpdateMovieDTO updates)
        {
            var movieToUpdate = await context.Movies.FindAsync(movieId);
            
            if(movieToUpdate == null)
            {
                return null;
            }

            if(!updates.Name.IsNullOrEmpty())
                movieToUpdate.Name = updates.Name;
            if (!updates.Description.IsNullOrEmpty())
                movieToUpdate.Description = updates.Description;
            if (!updates.DirectorName.IsNullOrEmpty())
                movieToUpdate.DirectorName = updates.DirectorName;
           
            try
            {
                if (!updates.ReleaseDate.IsNullOrEmpty())
                    movieToUpdate.ReleaseDate = DateTime.Parse(updates.ReleaseDate);
            }
            catch
            {
                return null;
            }

            if (!updates.ReleaseDate.IsNullOrEmpty())
                movieToUpdate.Length = updates.Length;

            var files = updates.files;
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    var isValidImage = IsImageValid(file);

                    if (isValidImage)
                    {
                        var uniqueFileName = GetUniqueFileName(file);
                        var filePath = SaveImageToFile(file, uniqueFileName);

                        var image = await SaveImageToDatabase(filePath, context);

                        await SaveMovieImageToDatabase(movieToUpdate.Id, image.Id, context);
                    }
                }
            }

            await context.SaveChangesAsync();

            return movieToUpdate;
        }


        /// <summary>
        /// Updates a rating in the database.
        /// </summary>
        /// <param name="ratingId">The ID of the rating to update.</param>
        /// <param name="userId">The ID of the user updating the rating.</param>
        /// <param name="rating">The new rating value.</param>
        /// <returns>The updated rating.</returns>
        public async Task<Rating> UpdateRating(int ratingId, string userId, int rating)
        {
            var ratingToUpdate = await context.Ratings.Where(r => r.Id == ratingId && r.UserId == userId).FirstOrDefaultAsync();

            if (ratingToUpdate == null)
            {
                return null;
            }

            if(rating >= 1 && rating <= 10) 
            { 
                ratingToUpdate.RatingScore = rating;
                
                await context.SaveChangesAsync();
                return ratingToUpdate;
            }

            return null;
        }


        /// <summary>
        /// Deletes an image associated with a movie from the database.
        /// </summary>
        /// <param name="imageId">The ID of the image to delete.</param>
        /// <param name="movieId">The ID of the movie associated with the image.</param>
        /// <returns>True if the image is deleted successfully; otherwise, false.</returns>
        public async Task<bool> DeleteImage(int imageId, int movieId)
        {
            var image = await context.Images.FindAsync(imageId);
            var moviesImages = await context.MoviesImages.Where(mi => mi.ImageId == imageId && mi.MovieId == movieId).ToListAsync();
            
            if(image != null && moviesImages.Any())
            {
                context.Images.Remove(image);
                context.MoviesImages.RemoveRange(moviesImages);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }


        //Private methods


        /// <summary>
        /// Checks if an uploaded image file is valid.
        /// </summary>
        /// <param name="file">The uploaded image file.</param>
        /// <returns>True if the image is valid; otherwise, false.</returns>
        private bool IsImageValid(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var validExtensions = new[] { ".jpg", ".jpeg", ".png" };

            return file != null && file.Length > 0 && validExtensions.Contains(extension);
        }


        /// <summary>
        /// Generates a unique file name for an uploaded image file.
        /// </summary>
        /// <param name="file">The uploaded image file.</param>
        /// <returns>The unique file name.</returns>
        private string GetUniqueFileName(IFormFile file)
        {
            return $"{Guid.NewGuid()}_{file.FileName}";
        }


        /// <summary>
        /// Saves an uploaded image file to the server.
        /// </summary>
        /// <param name="file">The uploaded image file.</param>
        /// <param name="uniqueFileName">The unique file name.</param>
        /// <returns>The URL of the saved image file.</returns>
        private string SaveImageToFile(IFormFile file, string uniqueFileName)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Movies");
            var filePath = Path.Combine(folder, uniqueFileName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $"http://localhost:5099/Images/Movies/{uniqueFileName}";
        }


        /// <summary>
        /// Saves an image record to the database.
        /// </summary>
        /// <param name="filePath">The file path of the saved image.</param>
        /// <param name="context">The DataContext.</param>
        /// <returns>The saved Image object.</returns>
        private async Task<Image> SaveImageToDatabase(string filePath, DataContext context)
        {
            var image = new Image { Path = filePath };
            await context.Images.AddAsync(image);
            await context.SaveChangesAsync();

            return image;
        }


        /// <summary>
        /// Saves a movie-image association to the database.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <param name="imageId">The ID of the image.</param>
        /// <param name="context">The DataContext.</param>
        /// <returns></returns>
        private async Task SaveMovieImageToDatabase(int movieId, int imageId, DataContext context)
        {
            var movieImage = new MoviesImages { MovieId = movieId, ImageId = imageId };
            await context.MoviesImages.AddAsync(movieImage);
            await context.SaveChangesAsync();
        }
    }
}
