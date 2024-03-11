using Microsoft.AspNetCore.Identity;

namespace MovieAPI.Data.Models
{
    //By inheriting the IdntityUser class, we will have the email and password fields by default
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Comments = new HashSet<Comment>();
            this.Ratings = new HashSet<Rating>();
        }

        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Rating> Ratings { get; set; } = null!;
    }
}
