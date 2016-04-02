using AdventureWorks.SkiResort.Infrastructure.Model.Enums;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdventureWorks.SkiResort.Infrastructure.Model
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool FamilyFriendly { get; set; }

        public bool TakeAway { get; set; }

        public PriceLevel PriceLevel { get; set; }

        public FoodType FoodType { get; set; }

        public Noise LevelOfNoise { get; set; }

        public byte[] Photo { get; set; }
    }
}
