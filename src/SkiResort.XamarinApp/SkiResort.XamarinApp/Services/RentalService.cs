using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
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
            var httpService = new HTTPService(Config.API_URL);
            var rentalsData = await httpService.Get("/rentals");
            var rentals = JsonConvert.DeserializeObject<List<Rental>>(rentalsData);
            return rentals;
        }

        public async Task SaveRental(Rental rental)
        {
            var httpService = new HTTPService(Config.API_URL);
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            jsonSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" });
            var rentalJson = JsonConvert.SerializeObject(rental, jsonSettings);
            await httpService.Post("/rentals", rentalJson);
        }
    }
}
