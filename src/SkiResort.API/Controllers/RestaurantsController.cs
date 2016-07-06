using AdventureWorks.SkiResort.Infrastructure.AzureSearch;
using AdventureWorks.SkiResort.Infrastructure.Model;
using AdventureWorks.SkiResort.Infrastructure.Repositories;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using AdventureWorks.SkiResort.Infrastructure.Helpers;

namespace AdventureWorks.SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantsController : Controller
    {
        private readonly RestaurantsRepository _restaurantsRepository = null;
        private readonly RestaurantsSearchService _restaurantsSearchService = null;
        private readonly IConfigurationRoot _configuration = null;
        private const int _restaurantsCount = 10;
        private const int _recommendationsCount = 3;


        public RestaurantsController(IConfigurationRoot configuration, RestaurantsRepository restaurantsRepository,
            RestaurantsSearchService restaurantsSearchService)
        {
            _restaurantsRepository = restaurantsRepository;
            _restaurantsSearchService = restaurantsSearchService;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<Restaurant> GetAsync(int id)
        {
            return await _restaurantsRepository.GetAsync(id);
        }

        [HttpGet]
        [Route("nearby")]
        public async Task<IEnumerable<Restaurant>> GetNearByAsync(double latitude, double longitude)
        {
            if (!IsAzureSearchAvailable())
                return await _restaurantsRepository.GetNearByAsync(latitude, longitude, _restaurantsCount);
            else
            {
                List<Restaurant> restaurants = await _restaurantsSearchService.GetNearByAsync(_restaurantsCount);
                return restaurants
                    .OrderBy(r => MathCoordinates.GetDistance(r.Latitude, r.Longitude, latitude, longitude, 'M'));
            }
        }

        [HttpGet]
        [Route("recommendations/{searchtext}")]
        public Task<List<int>> GetRecommendationsAsync(string searchtext)
        {
            if (IsAzureSearchAvailable())
                return _restaurantsSearchService.GetRecommendationsAsync(searchtext, _recommendationsCount);

            return Task.FromResult(new List<int>()); // No recommendations.
        }

        [HttpGet]
        [Route("photo/{id}")]
        [ResponseCache(Duration = 180)]
        public async Task<IActionResult> GetPhotoAsync(int id)
        {
            var image = await _restaurantsRepository.GetPhotoAsync(id);
            if (image == null)
            {
                return HttpBadRequest();
            }
            return new FileStreamResult(new MemoryStream(image), "image/jpeg");
        }

        private bool IsAzureSearchAvailable()
        {
            return _configuration.Get<bool>("SearchConfig:UseAzureSearch");
        }
    }
}
