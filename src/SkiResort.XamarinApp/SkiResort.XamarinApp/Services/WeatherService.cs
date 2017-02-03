using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using SkiResort.XamarinApp.Entities;

namespace SkiResort.XamarinApp.Services
{
    class WeatherService
    {
        private HTTPService _httpService;
        public WeatherService()
        {
            _httpService = HTTPService.Instance;
        }
        public async Task<WeatherSummary> GetSummary()
        {
            var response = await _httpService.Get("/api/summaries");

            WeatherSummary summary = null;

            if (response.IsSuccessful)
                summary = JsonConvert.DeserializeObject<WeatherSummary>(response.Content);

            return summary;
        }
    }
}
