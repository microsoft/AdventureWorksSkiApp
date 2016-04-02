
using AdventureWorks.SkiResort.Infrastructure.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace AdventureWorks.SkiResort.Infrastructure.Context
{
    public class SkiResortContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Summary> Summaries { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }
    
        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Lift> Lifts { get; set; }
    }
}
