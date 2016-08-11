
using AdventureWorks.SkiResort.Infrastructure.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.Infrastructure.Context
{
    public class SkiResortContext : IdentityDbContext<ApplicationUser>
    {
        public SkiResortContext(DbContextOptions<SkiResortContext> options)
            :base(options)
        {
        }
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
