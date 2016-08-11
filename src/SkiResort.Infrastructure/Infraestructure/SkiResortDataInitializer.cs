
using AdventureWorks.SkiResort.Infrastructure.Context;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using AdventureWorks.SkiResort.Infrastructure.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace AdventureWorks.SkiResort.Infrastructure.Infraestructure
{
    public class SkiResortDataInitializer
    {
        private static readonly Random Randomize = new Random();
        private static string DefaultEmail = string.Empty;

        public async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var db = serviceProvider.GetService<SkiResortContext>())
            {
                var databaseCreated = await db.Database.EnsureCreatedAsync();
                if (databaseCreated)
                {
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
            var env = serviceProvider.GetService<IHostingEnvironment>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
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
                    BaseDepth = 42,
                    MaxTemperature = 43,
                    MinTemperature = 27,
                    Wind = 7,
                    Weather = Model.Enums.Weather.Snowing
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
                    Rating = 3,
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
                    Rating = 4,
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
                    Rating = 5,
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
                    Rating = 3,
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
                    Rating = 4,
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
                    Rating = 5,
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
                    Photo = GetRestaurant(7),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Low,
                    Rating = 6,
                    TakeAway = false
                },
                new Restaurant()
                {
                    Name = "SQL Server Bar",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = false,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.833847,
                    Longitude = -74.407326,
                    LevelOfNoise = Model.Enums.Noise.Medium,
                    Photo = GetRestaurant(8),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Medium,
                    Rating = 4,
                    TakeAway = false
                },
                new Restaurant()
                {
                    Name = "SQL Server Grill",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = false,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.833847,
                    Longitude = -74.407326,
                    LevelOfNoise = Model.Enums.Noise.Medium,
                    Photo = GetRestaurant(9),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Medium,
                    Rating = 4,
                    TakeAway = false
                },
                new Restaurant()
                {
                    Name = "SQL Server Café",
                    Description = defaultDescription,
                    Address = defaultAddress,
                    FamilyFriendly = false,
                    FoodType = Model.Enums.FoodType.American,
                    Latitude = 40.723847,
                    Longitude = -74.007326,
                    LevelOfNoise = Model.Enums.Noise.Low,
                    Photo = GetRestaurant(10),
                    Phone = "5555-5555",
                    PriceLevel = Model.Enums.PriceLevel.Medium,
                    Rating = 4,
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

            string[] types = { "Bar", "Grill", "Café", "Snack Shack", "BrewPub", "Restaurant" };

            int count = 0;
            foreach (var azureword in azurewords)
            {
                for (int type = 1; type <= 6; type++)
                {
                    var restaurant = new Restaurant()
                    {
                        Name = $"{azureword} {types[type - 1]}",
                        Description = "Enjoy fine food and attentive service. We only serve the freshest ingredients cooked to perfection.",
                        Address = "15 Ski App Way, Redmond Heights Way, Washington, USA",
                        FamilyFriendly = count % 2 != 0 ? false : true,
                        FoodType = count % 2 != 0 ? Model.Enums.FoodType.American : Model.Enums.FoodType.Spanish,
                        Latitude = 40.833847,
                        Longitude = -74.407326,
                        LevelOfNoise = count % 2 != 0 ? Model.Enums.Noise.Low : Model.Enums.Noise.Medium,
                        Photo = GetRestaurant(type),
                        Phone = "5555-5555",
                        PriceLevel = count % 2 != 0 ? Model.Enums.PriceLevel.Hight : Model.Enums.PriceLevel.Medium,
                        Rating = count % 2 != 0 ? 3 : 4,
                        TakeAway = count % 2 != 0 ? false : true,
                    };
                    count++;
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
                    WaitingTime = 12
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
                    WaitingTime = 5
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
                    WaitingTime = 2
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
                    WaitingTime = 2
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
                    WaitingTime = 3
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
                    WaitingTime = 0
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
                    WaitingTime = 0
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
                    WaitingTime = 0
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
                    PickupHour = 7,
                    ShoeSize = 7,
                    PoleSize = 22,
                    SkiSize = 20,
                    TotalCost = 50
                },
                new Rental()
                {
                    StartDate = defaultDate.AddMonths(1),
                    EndDate = defaultDate.AddMonths(1),
                    UserEmail = DefaultEmail,
                    Activity = Model.Enums.RentalActivity.Snowboard,
                    Category = Model.Enums.RentalCategory.Beginner,
                    Goal = Model.Enums.RentalGoal.Demo,
                    PickupHour = 8,
                    ShoeSize = 9,
                    PoleSize = 60,
                    SkiSize = 60,
                    TotalCost = 65
                },
                new Rental()
                {
                    StartDate = defaultDate.AddMonths(1),
                    EndDate = defaultDate.AddMonths(1),
                    UserEmail = DefaultEmail,
                    Activity = Model.Enums.RentalActivity.Snowboard,
                    Category = Model.Enums.RentalCategory.Beginner,
                    Goal = Model.Enums.RentalGoal.Demo,
                    PickupHour = 9,
                    ShoeSize = 10,
                    PoleSize = 80,
                    SkiSize = 80,
                    TotalCost = 70
                }
            };

            context.Rentals.AddRange(rentals);
            await context.SaveChangesAsync();
        }

        private static byte[] GetRestaurant(int index)
        {
            return Convert.FromBase64String(RestaurantPhotos.Restaurants[index - 1]);
        }
    }
}
