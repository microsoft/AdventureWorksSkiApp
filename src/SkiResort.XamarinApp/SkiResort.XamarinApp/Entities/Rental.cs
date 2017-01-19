using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Entities
{
    public class Rental
    {
        public int RentalId { get; set; }

        public string UserEmail { get; set; }

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

    }

    public enum RentalActivity
    {
        Ski = 0,
        Snowboard = 1
    }

    public enum RentalCategory
    {
        Unknown = 0,
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }

    public enum RentalGoal
    {
        Unknown = 0,
        Demo = 1,
        Performance = 2,
        Sport = 3,
    }
}
