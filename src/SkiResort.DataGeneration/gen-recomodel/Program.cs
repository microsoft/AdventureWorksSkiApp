using Recommendations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace gen_recomodel
{
    class Program
    {
        private const string BaseUri = "https://westus.api.cognitive.microsoft.com/recommendations/v4.0";

        static void Main(string[] args)
        {
            const int users = 100;

            List<RestaurantFeatures> restaurants = GetRestaurantFeatures().ToList();
            RestaurantsToCsv(restaurants, "restaurants.txt");
            HistoryToCsv(GetReservationsHistory(restaurants, users), "history.txt");

            RecommendationsApiWrapper recos = new RecommendationsApiWrapper(ConfigurationManager.AppSettings["RecoKey"], BaseUri);
            ModelInfo modelInfo = recos.CreateModel("diningprefs");
            Console.WriteLine($"Model created, modelId: {modelInfo.Id}");
            recos.UploadCatalog(modelInfo.Id, "restaurants.txt", "RestaurantsCatalog");
            recos.UploadUsage(modelInfo.Id, "history.txt", "RestaurantsUsage");

            BuildRequestInfo requestInfo = new BuildRequestInfo
            {
                Description = "build of " + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                BuildType = BuildType.Recommendation,
                BuildParameters = new BuildParameters
                {
                    Recommendation = new RecommendationBuildParameters
                    {
                        NumberOfModelIterations = 10,
                        NumberOfModelDimensions = 20,
                        ItemCutOffLowerBound = 1,
                        EnableModelingInsights = true,
                        EnableU2I = true,
                        UseFeaturesInModel = false,
                        AllowColdItemPlacement = true,
                        EnableFeatureCorrelation = false
                    }
                }
            };
            string operationLocationHeader;
            long buildId = recos.BuildModel(modelInfo.Id, requestInfo, out operationLocationHeader);

            // Monitor the build and wait for completion.
            Console.WriteLine("Monitoring build {0}", buildId);
            var buildInfo = recos.WaitForOperationCompletion<BuildInfo>(RecommendationsApiWrapper.GetOperationId(operationLocationHeader));
            Console.WriteLine("Build {0} ended with status {1}.\n", buildId, buildInfo.Status);

            if (String.Compare(buildInfo.Status, "Succeeded", StringComparison.OrdinalIgnoreCase) != 0)
            {
                Console.WriteLine("Build {0} did not end successfully, the sample app will stop here.", buildId);
                return;
            }

            // Waiting  in order to propagate the model updates from the build...
            Console.WriteLine("Waiting for 40 sec for propagation of the built model...");
            Thread.Sleep(TimeSpan.FromSeconds(40));

            // The below api is more meaningful when you want to give a certain build id to be an active build.
            // Currently this app has a single build which is already active.
            Console.WriteLine("Setting build {0} as active build.", buildId);
            recos.SetActiveBuild(modelInfo.Id, buildId);

            Console.WriteLine("Model ready");

            while (true)
            {
                string restaurantId = Console.ReadLine();
                var recommendations =  recos.GetRecommendations(modelInfo.Id, null, restaurantId, 3);

                foreach (var r in recommendations.RecommendedItemSetInfo)
                {
                    foreach (var item in r.Items)
                    {
                        Console.WriteLine($"Name: {item.Name}, Id: {item.Id}, Rating: {r.Rating}, Reasoning: {r.Reasoning.First()}");
                    }
                }

                Console.WriteLine();
            }
        }

        private static void RestaurantsToCsv(IEnumerable<RestaurantFeatures> restaurants, string filename)
        {
            using (StreamWriter w = new StreamWriter(filename, false, Encoding.UTF8))
            {
                foreach (RestaurantFeatures r in restaurants)
                {
                    w.WriteLine($"{r.RestaurantId},{r.Name},,,FamilyFriendly={r.FamilyFriendly},FoodType={r.FoodType},LevelOfNoise={r.LevelOfNoise},PriceLevel={r.PriceLevel},Rating={r.Rating},TakeAway={r.TakeAway}");
                }
            }
        }

        private static IEnumerable<RestaurantFeatures> GetRestaurantFeatures()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT RestaurantId,Name,FamilyFriendly,FoodType,LevelOfNoise,PriceLevel,Rating,TakeAway FROM Restaurants", con);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return new RestaurantFeatures
                    {
                        RestaurantId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        FamilyFriendly = reader.GetBoolean(2),
                        FoodType = reader.GetInt32(3),
                        LevelOfNoise = reader.GetInt32(4),
                        PriceLevel = reader.GetInt32(5),
                        Rating = reader.GetDouble(6),
                        TakeAway = reader.GetBoolean(7)
                    };
                }
            }
        }

        private static IEnumerable<Tuple<int, int>> GetReservationsHistory(List<RestaurantFeatures> restaurants, int userCount)
        {
            Random random = new Random(12345);

            for (int user = 0; user < userCount; user++)
            {
                // Pick a first random restaurant and then use its features to derive other preferred places
                int index = random.Next(0, restaurants.Count);
                RestaurantFeatures fav = restaurants[index];
                yield return Tuple.Create(user, fav.RestaurantId);

                int otherCount = random.Next(5, 11);
                for (int other = 0; other < otherCount; other++)
                {
                    IEnumerable<RestaurantFeatures> candidates = restaurants;

                    // If they cared about family friendly, assume they'll continue to care
                    if (restaurants[index].FamilyFriendly)
                    {
                        candidates = candidates.Where(r => r.FamilyFriendly);
                    }

                    // Similar price range
                    candidates = candidates.Where(r => Math.Abs(r.PriceLevel - fav.PriceLevel) <= 1);

                    // 80% change they like similar food type
                    if (random.NextDouble() < 0.8)
                    {
                        candidates = candidates.Where(r => r.FoodType == fav.FoodType);
                    }

                    // Same or higher rating
                    candidates = candidates.Where(r => r.Rating >= fav.Rating);

                    // Pick another restaurant they'd like
                    RestaurantFeatures next = candidates.Skip(random.Next(0, candidates.Count())).First();
                    yield return Tuple.Create(user, next.RestaurantId);
                }
            }
        }

        private static void HistoryToCsv(IEnumerable<Tuple<int, int>> history, string filename)
        {
            using (StreamWriter w = new StreamWriter(filename, false, Encoding.UTF8))
            {
                // No heading for recommendations service CSVs
                // w.WriteLine("UserId,RestaurantId");

                foreach (var h in history)
                {
                    w.WriteLine($"{h.Item1},{h.Item2}");
                }
            }
        }

        public class RestaurantFeatures
        {
            public int RestaurantId { get; set; }

            public string Name { get; set; }

            public double Rating { get; set; }

            public bool FamilyFriendly { get; set; }

            public bool TakeAway { get; set; }

            public int PriceLevel { get; set; }

            public int FoodType { get; set; }

            public int LevelOfNoise { get; set; }
        }
    }
}