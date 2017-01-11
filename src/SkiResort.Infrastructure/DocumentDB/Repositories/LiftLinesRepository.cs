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
            string databaseId = "skiresortliftlines";
            string collection = "liftlines";

            string query =
                $"SELECT TOP 10 * FROM liftlines ORDER BY time DESC";

            var parameters = new List<DBParameter>();

            string response = await ExecuteQuery(databaseId, collection, query, parameters);
            List<LiftLines> docs = JsonConvert.DeserializeObject<LiftLinesReponse>(response).Documents;
            return docs;
        }

        public async Task<List<LiftLinesArchive>> GetLiftWaitHistoryWaitingAsync(TimeSpan timeBack)
        {
            string databaseId = "skiresortliftlinesarchive";
            string collection = "liftlinesarchive";

            string query =
                $" SELECT TOP 10 * FROM liftlinesarchive WHERE time >= @timeBack ORDER BY time DESC";

            var parameters = new List<DBParameter>();
            DateTimeOffset time = DateTimeOffset.UtcNow.Add(-timeBack);
            parameters.Add(new DBParameter() { name = "@timeBack", value = $"'{time:o}'" });

            string response = await ExecuteQuery(databaseId, collection, query, parameters);
            List<LiftLinesArchive> docs = JsonConvert.DeserializeObject<LiftLinesArchiveResponse>(response).Documents;

            return docs;
        }

    }
}
