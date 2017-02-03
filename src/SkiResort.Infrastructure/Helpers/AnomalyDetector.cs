using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventureWorks.SkiResort.Infrastructure.Helpers
{
    public class AnomalyDetector
    {
        private static HttpClient _client;

        public static void Initialize(IConfiguration configuration)
        {
            string key = configuration["AnomalyDetection:Key"];
            string requestUri = configuration["AnomalyDetection:RequestUri"];

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            _client.BaseAddress = new Uri(requestUri);
        }

        public static async Task<bool> SlowChairliftAsync(IEnumerable<Tuple<DateTimeOffset, int>> data)
        {
            // A real model would have a more sophisticated heuristic to interpret the output. At a minimum it should checkt to
            // see a) if the slowdown is at the beginning or end of the time range and whether it's getting slower or faster. Here 
            // we simply look at the values and if we see more than 2 spikes (in either direction) we consider it "suspect" and 
            // flag the chairlift.
            int[] spikes = await AnalyzeAsync(data);
            bool slow = spikes.Length > 2;
            return slow;
        }

        public static async Task<int[]> AnalyzeAsync(IEnumerable<Tuple<DateTimeOffset, int>> data)
        {
            List<int> result = new List<int>();

            try
            {
                var inputs = new List<Dictionary<string, string>>();
                foreach (var d in data)
                {
                    inputs.Add(
                        new Dictionary<string, string>()
                        {
                            { "Time", d.Item1.ToString(("MM/dd/yy HH:mm:ss")) },
                            { "Data", d.Item2.ToString() },
                        });
                }

                if (inputs.Count > 0)
                {
                    var request = new
                    {
                        Inputs = new Dictionary<string, List<Dictionary<string, string>>>()
                        {
                            {
                                "input1", inputs
                            },
                        },
                        GlobalParameters = new Dictionary<string, string>()
                        {
                            {
                                "tspikedetector.sensitivity", "3"
                            },
                            {
                                "zspikedetector.sensitivity", "3"
                            },
                        }
                    };

                    HttpResponseMessage response = await _client.PostAsync("", new StringContent(JsonConvert.SerializeObject(request)));
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        dynamic json = JObject.Parse(content);

                        for (int i = 0; i < json.Results.output1.Count; i++)
                        {
                            int tspike = 0;
                            if (int.TryParse(json.Results.output1[i].TSpike.Value, out tspike) && tspike > 0)
                            {
                                result.Add(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            return result.ToArray();
        }
    }
}
