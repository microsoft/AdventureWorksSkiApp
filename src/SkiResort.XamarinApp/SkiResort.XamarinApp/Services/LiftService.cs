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
        public async Task<List<Lift>> GetLifts()
        {
            var httpService = new HTTPService(Config.API_URL);
            var liftsData = await httpService.Get(
                string.Format("/lifts/nearby?latitude={0}&longitude={1}",
                    Config.USER_DEFAULT_POSITION_LATITUDE,
                    Config.USER_DEFAULT_POSITION_LONGITUDE));
            var lifts = JsonConvert.DeserializeObject<List<Lift>>(liftsData);
            return lifts;
        }
    }
}
