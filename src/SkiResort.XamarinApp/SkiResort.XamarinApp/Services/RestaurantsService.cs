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
            var response = await _httpService.Get(
                string.Format("/api/restaurants/nearby?latitude={0}&longitude={1}",
                    Config.USER_DEFAULT_POSITION_LATITUDE,
                    Config.USER_DEFAULT_POSITION_LONGITUDE));

            var restaurants = new List<Restaurant>();

            if (response.IsSuccessful)
                restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(response.Content);

            return restaurants;
        }

        public async Task<Restaurant> GetRestaurant(int restaurantId)
        {
            var response = await _httpService.Get(string.Format("/api/restaurants/{0}", restaurantId));

            Restaurant restaurant = null;

            if (response.IsSuccessful)
                restaurant = JsonConvert.DeserializeObject<Restaurant>(response.Content);

            return restaurant;
        }

        private async Task<List<int>> GetRecommendedRestaurantsIds(int restaurantId)
        {
            var response = await _httpService.Get(string.Format("/api/restaurants/recommendations/{0}", restaurantId));

            var ids = new List<int>();

            if (response.IsSuccessful)
                ids = JsonConvert.DeserializeObject<List<int>>(response.Content);

            return ids;
        }

        public async Task<List<Restaurant>> GetRecommendedRestaurants(int restaurantId)
        {
            var restaurantIds = await GetRecommendedRestaurantsIds(restaurantId);
            var taskList = new List<Task<Restaurant>>();
            var result = new List<Restaurant>();

            foreach(var id in restaurantIds)
            {
                taskList.Add(GetRestaurant(id));
            }

            foreach(var task in taskList)
            {
                var restaurant = await task;
                if (restaurant != null)
                    result.Add(restaurant);
            }

            return result;
        }
    }
}
