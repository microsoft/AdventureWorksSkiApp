using System;

namespace AdventureWorks.SkiResort.Infrastructure.Model
{
    public class WeatherHistory
    {
        public DateTimeOffset Date { get; set; }

        public bool Snow { get; set; }
    }
}
