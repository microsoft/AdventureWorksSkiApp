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
            var httpService = new HTTPService("http://adventureworkskiresort.azurewebsites.net/api");
            var restaurantsData = await httpService.Get("/restaurants/nearby?latitude=0&longitude=0");
            var restaurants = JsonConvert.DeserializeObject<List<Restaurant>>(restaurantsData);
            return restaurants;
        }
    }
}
