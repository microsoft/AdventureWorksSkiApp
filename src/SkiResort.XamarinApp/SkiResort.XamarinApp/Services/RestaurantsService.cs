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
        public async Task<List<Restaurant>> GetRestaurants()
        {
            var httpService = new HTTPService(Config.API_URL);
            var restaurantsData = await httpService.Get(
                string.Format("/restaurants/nearby?latitude={0}&longitude={1}",
                    Config.USER_DEFAULT_POSITION_LATITUDE,
                    Config.USER_DEFAULT_POSITION_LONGITUDE));
            var restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(restaurantsData);
            return restaurants;
        }
    }
}
