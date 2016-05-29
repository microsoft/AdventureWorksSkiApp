using AdventureWorks.SkiResort.Infrastructure.Context;
using AdventureWorks.SkiResort.Infrastructure.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.Infrastructure.Infraestructure
{
    public class SkiResortDataInitializer
    {
        private static readonly Random Randomize = new Random(12345); // Stable seed for repeatable data generation
        private static string DefaultEmail = string.Empty;

        public async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var db = serviceProvider.GetService<SkiResortContext>())
            {
                var databaseCreated = await db.Database.EnsureCreatedAsync();
                if (databaseCreated)
                {
                    await CreateAnalyticsDatabaseObjects(db);
                    await InitializeDatabaseData(serviceProvider, db);
                }
            }
        }

        async Task InitializeDatabaseData(IServiceProvider serviceProvider, SkiResortContext context)
        {
            await CreateDefaultUser(serviceProvider);
            await CreateSummaries(context);
            await CreateRestaurants(context);
            await CreateAdditionalRestaurant(context);
            await CreateLifts(context);
            await CreateRentals(context);
        }

        private static async Task CreateDefaultUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var applicationEnvironment = serviceProvider.GetService<IApplicationEnvironment>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(applicationEnvironment.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var user = await userManager.FindByNameAsync(configuration["DefaultUsername"]);
            if (user == null)
            {
                DefaultEmail = configuration["DefaultEmail"];
                user = new ApplicationUser
                {
                    UserName = configuration["DefaultUsername"],
                    Photo = Convert.FromBase64String(UserPhotos.Photos[0]), // Default user photo
                    FullName = configuration["DefaultFullName"],
                    Email = DefaultEmail
                };

                var result = await userManager.CreateAsync(user, configuration["DefaultPassword"]);
            }
        }

        async Task CreateSummaries(SkiResortContext context)
        {
            var summaries = new List<Summary>();

            for (int i = 1; i < 24 * 60; i++)
            {
                var summary = new Summary()
                {
                    DateTime = DateTime.UtcNow.AddHours(i),
                    BaseDepth = Randomize.Next(40, 60),
                    MaxTemperature = Randomize.Next(40, 50),
                    MinTemperature = Randomize.Next(10, 27),
                    Wind = Randomize.Next(5, 10),
                    Weather = (Model.Enums.Weather)Randomize.Next(1, 4)
                };

                summaries.Add(summary);
            }

            context.Summaries.AddRange(summaries);
            await context.SaveChangesAsync();
        }

        async Task CreateRestaurants(SkiResortContext context)
        {
            string defaultDescription = "By 1950, the restaurant had spread throughout the East, Midwest and Southwest United States. It offers sizzlers, hand-cut steaks, steak combos, seafood, ribs and chicken products, pastas, entrées, burgers and sandwiches, lunch meals, sides, salads, soups, baked goods, hot appetizers, and desserts, as well as sizzlers for kids.";
            string defaultAddress = "15 Ski App Way, Redmond Heights Way, Washington, USA";

            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "Azure Cafe",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = true,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.721847,
                    Longitude = -74.004326,
                    LevelOfNoise = Model.Enums.Noise.Loud,
                    Photo = GetRestaurant(1),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Low,
                    Rating = Randomize.Next(3, 5),
                    TakeAway = true
                },
                new Restaurant()
                {
                    Name = "HDInsight Snack",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = true,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.721847,
                    Longitude = -74.003326,
                    LevelOfNoise = Model.Enums.Noise.Loud,
                    Photo = GetRestaurant(2),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Medium,
                    Rating = Randomize.Next(3, 5),
                    TakeAway = true
                },
                new Restaurant()
                {
                    Name = "Cloud Bar",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = false,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.721847,
                    Longitude = -74.001326,
                    LevelOfNoise = Model.Enums.Noise.Medium,
                    Photo = GetRestaurant(3),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Low,
                    Rating = Randomize.Next(3, 5),
                    TakeAway = false
                },
                new Restaurant()
                {
                    Name = "SQL Grill",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = true,
                    FoodType = Model.Enums.FoodType.Spanish,
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    LevelOfNoise = Model.Enums.Noise.Loud,
                    Photo = GetRestaurant(4),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Hight,
                    Rating = Randomize.Next(3, 5),
                    TakeAway = true
                },
                new Restaurant()
                {
                    Name = "Edge Snack",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = true,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    LevelOfNoise = Model.Enums.Noise.Medium,
                    Photo = GetRestaurant(5),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Low,
                    Rating = Randomize.Next(3, 5),
                    TakeAway = true
                },
                new Restaurant()
                {
                    Name = "Visual Studio Bar",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = false,
                    FoodType = Model.Enums.FoodType.Spanish,
                    Latitude = 40.721847,
                    Longitude = -74.001326,
                    LevelOfNoise = Model.Enums.Noise.Loud,
                    Photo = GetRestaurant(6),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Low,
                    Rating = Randomize.Next(3, 5),
                    TakeAway = false
                },
                new Restaurant()
                {
                    Name = "ASP.NET 5 Bar",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = false,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.723847,
                    Longitude = -74.007326,
                    LevelOfNoise = Model.Enums.Noise.Loud,
                    Photo = GetRestaurant(1),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Low,
                    Rating = Randomize.Next(3, 5),
                    TakeAway = false
                }
            };

            context.Restaurants.AddRange(restaurants);
            await context.SaveChangesAsync();
        }

        async Task CreateAdditionalRestaurant(SkiResortContext context)
        {
            string[] azurewords = {"SQL Server", "App Service", "Azure", "Virtual Machine", "Logic App", "Mobile App",
                "API App", "DocumentDB", "Redis", "Data Lake", "ServiceBus", "Storage", "Files", "Batch", "HPC"};

            string[] types = { "Bar", "Grill", "Café", "Snack Shack", "BrewPub", "Restaurant", "Tavern" };

            foreach (var azureword in azurewords)
            {
                foreach (var type in types)
                {
                    var restaurant = new Restaurant()
                    {
                        Name = $"{azureword} {type}",
                        Description = "Enjoy fine food and attentive service. We serve only the freshest ingredients cooked to perfection.",
                        Address = "15 Ski App Way, Redmond Heights Way, Washington, USA",
                        FamilyFriendly = Randomize.Next(0, 2) == 0 ? false : true,
                        FoodType = (Model.Enums.FoodType)Randomize.Next(1, 3),
                        Latitude = 40.733847,
                        Longitude = -74.307326,
                        LevelOfNoise = (Model.Enums.Noise)Randomize.Next(1, 4),
                        Photo = GetRestaurant(Randomize.Next(1, 6)),
                        Phone = "5555-5555",
                        PriceLevel = (Model.Enums.PriceLevel)Randomize.Next(1, 4),
                        Rating = Randomize.Next(3, 6),
                        TakeAway = false
                    };

                    context.Restaurants.Add(restaurant);
                    await context.SaveChangesAsync();
                }
            }


        }

        async Task CreateLifts(SkiResortContext context)
        {
            var lifts = new List<Lift>()
            {
                new Lift()
                {
                    Name = "Education Hill",
                    Latitude = 40.721847,
                    Longitude = -74.001326,
                    ClosedReason = string.Empty,
                    Rating = Model.Enums.LiftRating.Advanced,
                    Status =  Model.Enums.LiftStatus.Open,
                    StayAway = true,
                    WaitingTime = Randomize.Next(1, 20)
                },
                new Lift()
                {
                    Name = "Bear Creek",
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    ClosedReason = string.Empty,
                    Rating = Model.Enums.LiftRating.Intermediate,
                    Status =  Model.Enums.LiftStatus.Open,
                    StayAway = false,
                    WaitingTime = Randomize.Next(1, 20)
                },
                new Lift()
                {
                    Name = "Overlake Jump",
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    ClosedReason = string.Empty,
                    Rating = Model.Enums.LiftRating.Intermediate,
                    Status =  Model.Enums.LiftStatus.Open,
                    StayAway = false,
                    WaitingTime = Randomize.Next(1, 20)
                },
                new Lift()
                {
                    Name = "Belltown Express",
                    Latitude = 40.721847,
                    Longitude = -74.017326,
                    ClosedReason = string.Empty,
                    Rating = Model.Enums.LiftRating.Beginner,
                    Status =  Model.Enums.LiftStatus.Open,
                    StayAway = false,
                    WaitingTime = Randomize.Next(1, 20)
                },
                new Lift()
                {
                    Name = "Grass Lawn Caf",
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    ClosedReason = string.Empty,
                    Rating = Model.Enums.LiftRating.Beginner,
                    Status =  Model.Enums.LiftStatus.Open,
                    StayAway = false,
                    WaitingTime = Randomize.Next(1, 20)
                },
                new Lift()
                {
                    Name = "Redmond way",
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    ClosedReason = "Expect to open at noon",
                    Rating = Model.Enums.LiftRating.Advanced,
                    Status =  Model.Enums.LiftStatus.Closed,
                    StayAway = false,
                    WaitingTime = Randomize.Next(1, 20)
                },
                new Lift()
                {
                    Name = "Borealis",
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    ClosedReason = string.Empty,
                    Rating = Model.Enums.LiftRating.Intermediate,
                    Status =  Model.Enums.LiftStatus.Closed,
                    StayAway = false,
                    WaitingTime = Randomize.Next(1, 20)
                },
                new Lift()
                {
                    Name = "Bear Creek II",
                    Latitude = 40.721847,
                    Longitude = -74.007326,
                    ClosedReason = string.Empty,
                    Rating = Model.Enums.LiftRating.Intermediate,
                    Status =  Model.Enums.LiftStatus.Closed,
                    StayAway = false,
                    WaitingTime = Randomize.Next(1, 20)
                }

            };

            context.Lifts.AddRange(lifts);
            await context.SaveChangesAsync();
        }

        async Task CreateRentals(SkiResortContext context)
        {
            var defaultDate = DateTime.UtcNow;
            var rentals = new List<Rental>()
            {
                new Rental()
                {
                    StartDate = defaultDate,
                    EndDate = defaultDate,
                    UserEmail = DefaultEmail,
                    Activity = Model.Enums.RentalActivity.Ski,
                    Category = Model.Enums.RentalCategory.Advanced,
                    Goal = Model.Enums.RentalGoal.Performance,
                    PickupHour = Randomize.Next(7, 10),
                    ShoeSize = Randomize.Next(7, 10),
                    PoleSize = Randomize.Next(20, 80),
                    SkiSize = Randomize.Next(20, 80),
                    TotalCost = Randomize.Next(50, 200)
                },
                new Rental()
                {
                    StartDate = defaultDate.AddMonths(1),
                    EndDate = defaultDate.AddMonths(1),
                    UserEmail = DefaultEmail,
                    Activity = Model.Enums.RentalActivity.Snowboard,
                    Category = Model.Enums.RentalCategory.Beginner,
                    Goal = Model.Enums.RentalGoal.Demo,
                    PickupHour = Randomize.Next(7, 10),
                    ShoeSize = Randomize.Next(7, 10),
                    PoleSize = Randomize.Next(20, 80),
                    SkiSize = Randomize.Next(20, 80),
                    TotalCost = Randomize.Next(50, 200)
                },
                new Rental()
                {
                    StartDate = defaultDate.AddMonths(1),
                    EndDate = defaultDate.AddMonths(1),
                    UserEmail = DefaultEmail,
                    Activity = Model.Enums.RentalActivity.Snowboard,
                    Category = Model.Enums.RentalCategory.Beginner,
                    Goal = Model.Enums.RentalGoal.Demo,
                    PickupHour = Randomize.Next(7, 10),
                    ShoeSize = Randomize.Next(7, 10),
                    PoleSize = Randomize.Next(20, 80),
                    SkiSize = Randomize.Next(20, 80),
                    TotalCost = Randomize.Next(50, 200)
                }
            };

            context.Rentals.AddRange(rentals);
            await context.SaveChangesAsync();
        }

        private static byte[] GetRestaurant(int index)
        {
            return Convert.FromBase64String(RestaurantPhotos.Restaurants[index - 1]);
        }

        private static async Task<bool> IsAnalyticsEnabled(SkiResortContext context)
        {
            object r = await context.GetServerPropertyAsync("IsAdvancedAnalyticsInstalled");
            return r != null && (int)r > 0;
        }

        async Task CreateAnalyticsDatabaseObjects(SkiResortContext context)
        {
            if (!await IsAnalyticsEnabled(context))
            {
                return;
            }

            // Column store for rentals table
            await context.Database.ExecuteSqlCommandAsync(
            @"CREATE NONCLUSTERED COLUMNSTORE INDEX RentalsCounts ON Rental (StartDate)");

            // A place to store rental predictions trained model
            await context.Database.ExecuteSqlCommandAsync(
            @"CREATE TABLE Model (Serialized VARBINARY(MAX))");

            // View for rentals daily summary with dimensions joined in
            await context.Database.ExecuteSqlCommandAsync(
            @"CREATE VIEW RentalFeatures AS
            SELECT rr.Year, rr.Month, rr.Day, rr.RentalCount, DATEPART(WEEKDAY, DATEFROMPARTS(rr.Year, rr.Month, rr.Day)) WeekDay, CONVERT(BIT, IIF(h.Date IS NULL, 0, 1)) Holiday, w.Snow
            FROM
                (SELECT YEAR(r.StartDate) Year, MONTH(r.StartDate) Month, DAY(r.StartDate) Day, COUNT(*) RentalCount
                 FROM Rental r
                 GROUP BY YEAR(r.StartDate), MONTH(r.StartDate), DAY(r.StartDate)) rr
            LEFT OUTER JOIN Holiday h ON rr.Year = YEAR(h.Date) AND rr.Month = MONTH(h.Date) AND rr.Day = DAY(h.Date)
            LEFT OUTER JOIN WeatherHistory w ON rr.Year = YEAR(w.Date) AND rr.Month = MONTH(w.Date) AND rr.Day = DAY(w.Date)");

            await context.Database.ExecuteSqlCommandAsync(
            @"CREATE PROCEDURE TrainRentalModel AS
            BEGIN
	            DECLARE @q NVARCHAR(MAX) = N'SELECT * FROM RentalFeatures ORDER BY Year, Month, Day'
	            DECLARE @s NVARCHAR(MAX) = N'
            rentals = InputDataSet
            rentals$FHoliday = factor(rentals$Holiday)
            rentals$FSnow = factor(rentals$Snow)
            rentals$FWeekDay = factor(rentals$WeekDay)
            model = rpart(RentalCount ~ Month + Day + FWeekDay + FHoliday + FSnow, rentals)
            serialized = data.frame(model = as.raw(serialize(model, NULL)))'

	            BEGIN TRY
		            BEGIN TRANSACTION
		            DELETE FROM Model
		            INSERT INTO Model
		            EXEC sp_execute_external_script @language = N'R', @script = @s, @input_data_1 = @q, @output_data_1_name = N'serialized'
		            COMMIT
	            END TRY
	            BEGIN CATCH
		            PRINT 'Failed to recompute model, rolling back to previous trained model'
		            ROLLBACK
	            END CATCH
            END");

            await context.Database.ExecuteSqlCommandAsync(
            @"CREATE PROCEDURE PredictRentals @q NVARCHAR(MAX) AS
            BEGIN
	            DECLARE @serialized VARBINARY(MAX) = (SELECT TOP 1 Serialized FROM Model)
	            DECLARE @s NVARCHAR(MAX) = N'
            rentals = InputDataSet
            rentals$FHoliday = factor(rentals$Holiday)
            rentals$FSnow = factor(rentals$Snow)
            rentals$FWeekDay = factor(rentals$WeekDay)
            model = unserialize(as.raw(m))
            p = predict(model, rentals)
            OutputDataSet = data.frame(p)'

	            EXEC sp_execute_external_script @language = N'R', @script = @s, @input_data_1 = @q, @params = N'@m VARBINARY(MAX)', @m = @serialized
		            WITH RESULT SETS ((Predicted FLOAT))
            END");
        }
    }
}
