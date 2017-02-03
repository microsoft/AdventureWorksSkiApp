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
        private HTTPService _httpService;
        public RentalService()
        {
            _httpService = HTTPService.Instance;
        }
        public async Task<List<Rental>> GetRentals()
        {
            var response = await _httpService.Get("/api/rentals");

            var rentals = new List<Rental>();

            if (response.IsSuccessful)
                rentals = JsonConvert.DeserializeObject<List<Rental>>(response.Content);

            return rentals;
        }

        public async Task<bool> SaveRental(Rental rental)
        {
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            jsonSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" });
            var rentalJson = JsonConvert.SerializeObject(RentalDTOForCreation.Create(rental), jsonSettings);

            var response = await _httpService.Post("/api/rentals", rentalJson);

            return response.IsSuccessful;
        }

        public async Task<bool> CheckHighDemand(DateTime date)
        {
            var response = await _httpService.Get("/api/rentals/check_high_demand?date=" + date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            return response.Content == "true";
        }
    }

    public class RentalDTOForCreation
    {
        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public int PickupHour { get; set; }

        public RentalActivity Activity { get; set; }

        public RentalCategory Category { get; set; }

        public RentalGoal Goal { get; set; }

        public double ShoeSize { get; set; }

        public int SkiSize { get; set; }

        public int PoleSize { get; set; }

        public double TotalCost { get; set; }

        public static RentalDTOForCreation Create(Rental rental)
        {
            return new RentalDTOForCreation()
            {
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                PickupHour = rental.PickupHour,
                Activity = rental.Activity,
                Category = rental.Category,
                Goal = rental.Goal,
                ShoeSize = rental.ShoeSize,
                SkiSize = rental.SkiSize,
                PoleSize = rental.PoleSize,
                TotalCost = rental.TotalCost
            };
        }
    }
}
