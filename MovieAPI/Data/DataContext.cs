using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data.Models;

namespace MovieAPI.Data
{
    //Inheriting the IdentityDbContext<ApplicationUser> to have access to the user object
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<MoviesComments> MoviesComments { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<MoviesRatings> MoviesRatings { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<MoviesImages> MoviesImages { get; set; } = null!;
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MoviesComments>().HasKey(x => new { x.MovieId, x.CommentId });
            builder.Entity<MoviesRatings>().HasKey(x => new { x.MovieId, x.RatingId });
            builder.Entity<MoviesImages>().HasKey(x => new { x.MovieId, x.ImageId });


            //Configure the 2 differenct roles
            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            base.OnModelCreating(builder);
        }
    }
}
