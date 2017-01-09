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
            var httpService = new HTTPService("http://adventureworkskiresort.azurewebsites.net/api");
            var liftsData = await httpService.Get("/lifts/nearby?latitude=0&longitude=0");
            var lifts = JsonConvert.DeserializeObject<List<Lift>>(liftsData);
            return lifts;
        }
    }
}
