using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AdventureWorks.SkiResort.Infrastructure.DocumentDB.Helpers;
using Newtonsoft.Json;
using SkiResort.Infrastructure.DocumentDB.Model;

namespace AdventureWorks.SkiResort.Infrastructure.DocumentDB.Repositories
{
    public class LiftLinesRepository
        : DocumentDBRequest
    {

        public LiftLinesRepository(IConfigurationRoot configuration)
            : base(configuration)
        {

        }

        public async Task<List<LiftLines>> GetLiftSkiersWaitingAsync()
        {
            try
            {
                string databaseId = "skiresortliftlines";
                string collection = "liftlines";

                string query =
                    $"SELECT TOP 10 * FROM c order by c._ts desc";

                var parameters = new List<DBParameter>();

                string response = await ExecuteQuery(databaseId, collection, query, parameters);
                List<LiftLines> docs = JsonConvert.DeserializeObject<LiftLinesReponse>(response).Documents;
                return docs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<List<LiftLinesArchive>> GetLiftWaitHistoryWaitingAsync(TimeSpan timeBack)
        {
            try
            {
                string databaseId = "skiresortliftlinesarchive";
                string collection = "liftlinesarchive";

                TimeSpan t = DateTime.UtcNow.Add(-timeBack) - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;

                string query =
                    $"SELECT * FROM c WHERE c._ts >= {secondsSinceEpoch} order by c._ts desc";

                var parameters = new List<DBParameter>();
                string response = await ExecuteQuery(databaseId, collection, query, parameters);
                List<LiftLinesArchive> docs = JsonConvert.DeserializeObject<LiftLinesArchiveResponse>(response).Documents;

                return docs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

    }
}
