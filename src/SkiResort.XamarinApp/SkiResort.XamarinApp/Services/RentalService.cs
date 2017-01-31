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
            var rentalJson = JsonConvert.SerializeObject(RentalDTOForCreation.Create(rental), jsonSettings);
            await httpService.Post("/rentals", rentalJson);
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
