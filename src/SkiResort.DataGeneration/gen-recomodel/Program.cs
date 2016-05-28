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
        static void Main(string[] args)
        {
            const int users = 100;

            List<RestaurantFeatures> restaurants = GetRestaurantFeatures().ToList();
            RestaurantsToCsv(restaurants, "restaurants.txt");
            HistoryToCsv(GetReservationsHistory(restaurants, users), "history.txt");

            AzureMLRecommendations recos = new AzureMLRecommendations();
            recos.Init(ConfigurationManager.AppSettings["RecoUser"], ConfigurationManager.AppSettings["RecoKey"]);
            string modelId = recos.CreateModel("diningprefs");
            Console.WriteLine($"Model created, modelId: {modelId}");
            recos.ImportFile(modelId, "restaurants.txt", AzureMLRecommendations.Uris.ImportCatalog);
            recos.ImportFile(modelId, "history.txt", AzureMLRecommendations.Uris.ImportUsage);

            string buildId = recos.BuildModel(modelId, "build of " + DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

            AzureMLRecommendations.BuildStatus status;

            do
            {
                status = recos.GetBuildStatus(modelId, buildId);

                if (status == AzureMLRecommendations.BuildStatus.Cancelled ||
                    status == AzureMLRecommendations.BuildStatus.Error ||
                    status == AzureMLRecommendations.BuildStatus.Success)
                {
                    break;
                }

                Console.WriteLine("Waiting for model build to be completed.");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            } while (true);

            if (status != AzureMLRecommendations.BuildStatus.Success)
            {
                Console.WriteLine($"Model build failed, status: {status}");
                return;
            }

            Console.WriteLine("Model ready");

            while (true)
            {
                string restaurantId = Console.ReadLine();
                var recommendations =  recos.GetRecommendation(modelId, new List<string> { restaurantId }, 3);

                foreach (var r in recommendations)
                {
                    Console.WriteLine($"Name: {r.Name}, Id: {r.Id}, Rating: {r.Rating}, Reasoning: {r.Reasoning}");
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

                SqlCommand cmd = new SqlCommand("SELECT RestaurantId,Name,FamilyFriendly,FoodType,LevelOfNoise,PriceLevel,Rating,TakeAway FROM Restaurant", con);

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