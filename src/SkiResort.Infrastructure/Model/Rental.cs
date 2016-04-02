
using AdventureWorks.SkiResort.Infrastructure.Model.Enums;
using System;

namespace AdventureWorks.SkiResort.Infrastructure.Model
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
}
