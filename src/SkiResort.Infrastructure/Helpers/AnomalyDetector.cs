using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AdventureWorks.SkiResort.Infrastructure.Helpers
{
    public class AnomalyDetector
    {
        private const string ParamsString = "SpikeDetector.TukeyThresh=3; SpikeDetector.ZscoreThresh=3";
        private static readonly string ApiTemplate = "https://api.datamarket.azure.com/data.ashx/aml_labs/anomalydetection/v1/Score?data={0}&params={1}";
        private static HttpClient _client;

        public static void Initialize(IConfiguration configuration)
        {
            string key = configuration["Data:AnomalyDetection:Key"];
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"any:{key}"));
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
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

                StringBuilder buffer = new StringBuilder();
                foreach (var d in data)
                {
                    buffer.AppendFormat("{0:u}={1};", d.Item1, d.Item2);
                }

                if (buffer.Length > 0)
                {
                    string response = await _client.GetStringAsync(string.Format(ApiTemplate, Uri.EscapeDataString(buffer.ToString()), Uri.EscapeDataString(ParamsString)));
                    dynamic json = JsonConvert.DeserializeObject(response);
                    string table = (string)json.table;

                    string[] rows = table.Split(';');

                    for (int i = 1; i < rows.Length && !string.IsNullOrWhiteSpace(rows[i]); i++)
                    {
                        string[] values = rows[i].Split(',');
                        if (values.Length < 3)
                        {
                            break;
                        }

                        int tspike = 0;
                        if (int.TryParse(values[2], out tspike) && tspike > 0)
                        {
                            result.Add(i - 1);
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
