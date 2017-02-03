using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiResort.XamarinApp.Entities;

namespace SkiResort.XamarinApp.Services
{
    class LiftService
    {
        private HTTPService _httpService;
        public LiftService()
        {
            _httpService = HTTPService.Instance;
        }
        public async Task<List<Lift>> GetLifts()
        {
            var response = await _httpService.Get(
                string.Format("/api/lifts/nearby?latitude={0}&longitude={1}",
                    Config.USER_DEFAULT_POSITION_LATITUDE,
                    Config.USER_DEFAULT_POSITION_LONGITUDE));

            var result = new List<Lift>();

            if (response.IsSuccessful)
                result = JsonConvert.DeserializeObject<List<Lift>>(response.Content);

            return result;
        }
    }
}
