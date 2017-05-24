using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkiResort.Infrastructure.CosmosDB.Model
{
    public class LiftLinesArchive
    {
        [JsonProperty("pkey")]
        public string Pkey { get; set; }

        [JsonProperty("rkey")]
        public string Rkey { get; set; }

        [JsonProperty("skiercount")]
        public int Skiercount { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }
    }

    public class LiftLinesArchiveResponse
    {
        public string _rid { get; set; }

        public List<LiftLinesArchive> Documents { get; set; }

        public int count { get; set; }
    }
}
