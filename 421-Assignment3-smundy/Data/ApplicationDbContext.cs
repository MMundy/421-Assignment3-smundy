using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _421_Assignment3_smundy.Models;

namespace _421_Assignment3_smundy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<_421_Assignment3_smundy.Models.Movie> Movie { get; set; }
        public DbSet<_421_Assignment3_smundy.Models.Actor> Actor { get; set; }
        public DbSet<_421_Assignment3_smundy.Models.TweetSentiment> TweetSentiment { get; set; }
        public DbSet<_421_Assignment3_smundy.Models.MovieActor> MovieActor { get; set; }
    }
}