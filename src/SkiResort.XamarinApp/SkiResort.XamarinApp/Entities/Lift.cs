using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Entities
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

    public enum LiftStatus
    {
        Unknown = 0,
        Open = 1,
        Closed = 2
    }

    public enum LiftRating
    {
        Unknown = 0,
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }
}
