
using AdventureWorks.SkiResort.Infrastructure.Model.Enums;

namespace AdventureWorks.SkiResort.Infrastructure.Model
{
    public class Lift
    {
        public int LiftId { get; set; }

        public string Name { get; set; }

        public LiftRating Rating { get; set; }

        public LiftStatus Status { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool StayAway { get; set; }

        public int WaitingTime { get; set; }

        public string ClosedReason { get; set; }
    }
}
