using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data.Models;

namespace MovieAPI.Data
{
    public class DataContext : IdentityDbContext
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MoviesComments>().HasKey(x => new { x.MovieId, x.CommentId });
            builder.Entity<MoviesRatings>().HasKey(x => new { x.MovieId, x.RatingId });
            builder.Entity<MoviesImages>().HasKey(x => new { x.MovieId, x.ImageId });
            
            builder.Entity<IdentityRole>().HasData(new IdentityRole 
            { 
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", 
                Name = "Admin", 
                NormalizedName = "ADMIN".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            });
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = false,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Adminpass123@"),
                SecurityStamp = Guid.NewGuid().ToString(),
            });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb"
            });

            base.OnModelCreating(builder);
        }
    }
}
