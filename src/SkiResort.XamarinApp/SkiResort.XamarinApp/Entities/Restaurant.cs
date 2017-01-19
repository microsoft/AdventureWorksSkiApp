using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Entities
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

    public enum PriceLevel
    {
        Unknown = 0,
        Low = 1,
        Medium = 2,
        Hight = 3,
    }

    public enum FoodType
    {
        Unknown = 0,
        American = 1,
        Spanish = 2
    }

    public enum Noise
    {
        Unknown = 0,
        Low = 1,
        Medium = 2,
        Loud = 3
    }
}
