
using AdventureWorks.SkiResort.Infrastructure.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.Infrastructure.Context
{
    public class SkiResortContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WeatherHistory>().HasKey(w => w.Date);
            modelBuilder.Entity<Holiday>().HasKey(h => h.Date);
        }

        public DbSet<Summary> Summaries { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }
    
        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Lift> Lifts { get; set; }

        public DbSet<WeatherHistory> WeatherHistory { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public async Task<int?> EstimateRentalsAsync(DateTimeOffset date, bool snowedDayBefore, bool holiday)
        {
            // This is not the SQL string that will be executed by this command, it's a string
            // argument to the stored procedure. All numeric magnitudes, safe from injection
            // TODO: change the stored-proc to take individual arguments
            string sqlInput = string.Format("SELECT CONVERT(INT, {0}) AS Month, CONVERT(INT, {1}) AS Day, CONVERT(INT, {2}) AS WeekDay, CONVERT(BIT, {3}) AS Snow, CONVERT(BIT, {4}) AS Holiday",
                                            date.Month,
                                            date.Day,
                                            1 + (int)date.DayOfWeek,
                                            snowedDayBefore ? 1 : 0,
                                            holiday ? 1 : 0);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "PredictRentals";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@q", sqlInput));
            object r = await ExecuteSqlServerCommandAsync(cmd);
            return r == null ? (int?)null : (int?)(double)r;
        }

        public Task RetrainRentalsModelAsync()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "TrainRentalModel";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return ExecuteSqlServerCommandAsync(cmd);
        }

        public Task<object> GetServerPropertyAsync(string property)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT SERVERPROPERTY(@p) AS Property";
            cmd.Parameters.Add(new SqlParameter("@p", property));
            return ExecuteSqlServerCommandAsync(cmd);
        }

        private async Task<object> ExecuteSqlServerCommandAsync(SqlCommand cmd)
        {
            SqlConnection con = Database.GetDbConnection() as SqlConnection;
            if (con == null)
            {
                throw new NotSupportedException("Direct SQL commands not supported in the current configuration.");
            }

            bool closed = con.State == System.Data.ConnectionState.Closed;
            if (closed)
            {
                await con.OpenAsync();
            }

            try
            {
                cmd.Connection = con;
                return await cmd.ExecuteScalarAsync();
            }
            finally
            {
                if (closed)
                {
                    con.Close();
                }
            }
        }
    }
}
