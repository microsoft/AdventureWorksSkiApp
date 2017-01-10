using Newtonsoft.Json;
using SkiResort.XamarinApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiResort.XamarinApp.Services
{
    class RentalService
    {
        public async Task<List<Rental>> GetRentals()
        {
            var httpService = new HTTPService("http://adventureworkskiresort.azurewebsites.net/api");
            var rentalsData = await httpService.Get("/rentals");
            var rentals = JsonConvert.DeserializeObject<List<Rental>>(rentalsData);
            return rentals;
        }
    }
}
