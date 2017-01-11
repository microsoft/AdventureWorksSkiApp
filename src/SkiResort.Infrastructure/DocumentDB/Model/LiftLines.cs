using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SkiResort.Infrastructure.DocumentDB.Model
{
    public class LiftLines
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("skiercount")]
        public int SkierCount { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
       
    }

    public class LiftLinesReponse
    {
        public string _rid { get; set; }

        public List<LiftLines> Documents { get; set; }

        public int count { get; set; }
    }
}
