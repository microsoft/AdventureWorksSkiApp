using AdventureWorks.SkiResort.Infrastructure.Model.Enums;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdventureWorks.SkiResort.Infrastructure.Model
{
    public class Summary
    {
        public int SummaryId { get; set; }

        public DateTime DateTime { get; set; }

        public Weather Weather { get; set; }

        public int MaxTemperature { get; set; }

        public int MinTemperature { get; set; }

        public int Wind { get; set; }

        public int BaseDepth { get; set; }
    }
}
