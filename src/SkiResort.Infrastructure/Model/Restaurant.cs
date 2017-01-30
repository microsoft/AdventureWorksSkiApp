using AdventureWorks.SkiResort.Infrastructure.Model.Enums;
using Microsoft.Spatial;
using System.Collections.Generic;

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

        public Location Location
        {
            set
            {
                Longitude = value.coordinates[0];
                Latitude = value.coordinates[1];
            }
        }
    }

    public class Location
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public Crs crs { get; set; }
    }

    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }
    public class Properties
    {
        public string name { get; set; }
    }

}
