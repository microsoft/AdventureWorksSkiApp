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
        public async Task<WeatherSummary> GetSummary()
        {
            var httpService = new HTTPService(Config.API_URL);
            var summaryData = await httpService.Get("/summaries");
            var summary = JsonConvert.DeserializeObject<WeatherSummary>(summaryData);
            return summary;
        }
    }
}
