using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Recommendations;

namespace gen_restaurantssearch
{
    class Program
    {
        private const string BaseUri = "https://westus.api.cognitive.microsoft.com/recommendations/v4.0";

        static void Main(string[] args)
        {
            bool clear = false;

            SearchServiceClient client = new SearchServiceClient(ConfigurationManager.AppSettings["AzureSearchName"],
                                                                 new SearchCredentials(ConfigurationManager.AppSettings["AzureSearchKey"]));
            SearchIndexClient indexClient = client.Indexes.GetClient("restaurant");

            RecommendationsApiWrapper recos = new RecommendationsApiWrapper(ConfigurationManager.AppSettings["RecoKey"], BaseUri);
            string modelId = ConfigurationManager.AppSettings["RecoModelId"];

            List<IndexAction> actions = new List<IndexAction>();
            foreach (int id in GetCurrentRestaurantIds())
            {
                List<string> recommendedIds = new List<string>();
                if (!clear)
                {
                    var r = recos.GetRecommendations(modelId, null, id.ToString(), 3);
                    var ids = from info in r.RecommendedItemSetInfo
                              from item in info.Items
                              select item.Id;
                    recommendedIds.AddRange(ids);
                }
                actions.Add(IndexAction.Merge(new Document { { "RestaurantId", id.ToString() }, { "RecommendedIds", recommendedIds } }));
            }

            // Assume < 1000 actions, otherwise we'd need to split it in 1000-actions batches
            try
            {
                DocumentIndexResult indexResult = indexClient.Documents.Index(new IndexBatch(actions));
                int succeeded = indexResult.Results.Where(r => r.Succeeded).Count();
                Console.WriteLine($"Indexed completed. Items: {indexResult.Results.Count}, Succeeded: {succeeded}");
            }
            catch (IndexBatchException ex)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                Console.WriteLine(
                    "Failed to index some of the documents: {0}",
                    string.Join(", ", ex.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }
        }

        private static IEnumerable<int> GetCurrentRestaurantIds()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT TOP 20 RestaurantId FROM Restaurants", con);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return reader.GetInt32(0);
                }
            }
        }
    }
}
