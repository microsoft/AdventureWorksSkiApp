using Newtonsoft.Json;
using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Services
{
    class RestaurantsService
    {
        HTTPService _httpService;

        public RestaurantsService ()
        {
            _httpService = HTTPService.Instance;
        }

        public async Task<List<Restaurant>> GetRestaurants()
        {
            var restaurantsData = await _httpService.Get(
                string.Format("/restaurants/nearby?latitude={0}&longitude={1}",
                    Config.USER_DEFAULT_POSITION_LATITUDE,
                    Config.USER_DEFAULT_POSITION_LONGITUDE));
            var restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(restaurantsData);
            return restaurants;
        }

        public async Task<Restaurant> GetRestaurant(int restaurantId)
        {
            var data = await _httpService.Get(string.Format("/restaurants/{0}", restaurantId));
            var restaurant = JsonConvert.DeserializeObject<Restaurant>(data);
            return restaurant;
        }

        public async Task<List<Restaurant>> GetRecommendedRestaurants(int restaurantId)
        {
            var rawRestaurantIds = await _httpService.Get(string.Format("/restaurants/recommendations/{0}", restaurantId));
            var restaurantIds = JsonConvert.DeserializeObject<List<int>>(rawRestaurantIds);

            var taskList = new List<Task<Restaurant>>();
            var result = new List<Restaurant>();

            foreach(var id in restaurantIds)
            {
                taskList.Add(GetRestaurant(id));
            }

            foreach(var task in taskList)
            {
                result.Add(await task);
            }

            return result;
        }
    }
}
