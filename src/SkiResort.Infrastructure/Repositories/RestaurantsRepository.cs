
using AdventureWorks.SkiResort.Infrastructure.Context;
using AdventureWorks.SkiResort.Infrastructure.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.SkiResort.Infrastructure.Helpers;
using Microsoft.Data.Entity;
using System.Linq;

namespace AdventureWorks.SkiResort.Infrastructure.Repositories
{
    public class RestaurantsRepository
    {
        SkiResortContext _context;

        public RestaurantsRepository(SkiResortContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> GetAsync(int id)
        {
            var restaurant = await _context.Restaurants.SingleOrDefaultAsync(r => r.RestaurantId == id);
            return Build(restaurant);
        }


        public async Task<IEnumerable<Restaurant>> GetNearByAsync(double latitude, double longitude, int count)
        {
            return await _context.Restaurants
                .OrderBy(r => r.RestaurantId)
                .Select(r => new Restaurant()
                {
                    RestaurantId = r.RestaurantId,
                    Name = r.Name,
                    Description = r.Description,
                    Address = r.Address,
                    FamilyFriendly = r.FamilyFriendly,
                    FoodType = r.FoodType,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude,
                    LevelOfNoise = r.LevelOfNoise,
                    Phone = r.Phone,
                    PriceLevel = r.PriceLevel,
                    Rating = r.Rating,
                    TakeAway = r.TakeAway
                })
                .Take(count)
                .ToListAsync();
        }

        public async Task<byte[]> GetPhotoAsync(int id)
        {
            byte[] photo = null;
            var restaurant = await _context.Restaurants.SingleOrDefaultAsync(r => r.RestaurantId == id);
            if (restaurant != null)
                photo =  restaurant?.Photo;
            return photo;
        }

        Restaurant Build(Restaurant restaurant)
        {
            return new Restaurant()
            {
                RestaurantId = restaurant.RestaurantId,
                Name = restaurant.Name,
                Description = restaurant.Description,
                Address = restaurant.Address,
                FamilyFriendly = restaurant.FamilyFriendly,
                FoodType = restaurant.FoodType,
                Latitude = restaurant.Latitude,
                Longitude = restaurant.Longitude,
                LevelOfNoise = restaurant.LevelOfNoise,
                Phone = restaurant.Phone,
                PriceLevel = restaurant.PriceLevel,
                Rating = restaurant.Rating,
                TakeAway = restaurant.TakeAway
            };
        }
    }
}
