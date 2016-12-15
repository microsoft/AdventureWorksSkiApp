using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Services
{
    class WeatherService
    {
        public async Task<WeatherSummary> GetSummary()
        {
            var httpService = new HTTPService("http://adventureworkskiresort.azurewebsites.net/api");
            var summaryData = await httpService.Get("/summaries");
            var summary = JsonConvert.DeserializeObject<WeatherSummary>(summaryData);
            return summary;
        }
    }

    class WeatherSummary
    {
        public int MaxTemperature { get; set; }
        public int MinTemperature { get; set; }
        public int Wind { get; set; }
        public int BaseDepth { get; set; }
    }
}
