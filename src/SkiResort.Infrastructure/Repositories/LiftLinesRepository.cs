using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AdventureWorks.SkiResort.Infrastructure.Repositories
{
    public class LiftLinesRepository
    {
        private CloudTable _summaryTable;
        private CloudTable _historyTable;

        public LiftLinesRepository(CloudStorageAccount account)
        {
            _summaryTable = account.CreateCloudTableClient().GetTableReference("liftlines");
            _historyTable = account.CreateCloudTableClient().GetTableReference("liftlinesarchive");
        }

        public async Task<IEnumerable<Tuple<string, int?>>> LiftSkiersWaitingAsync()
        {
            // This table contains name and skier count for each lift, and it's updated continously
            // by Stream Analytics as it processes the location event stream
            var segment = await _summaryTable.ExecuteQuerySegmentedAsync(new TableQuery(), null);

            return segment.Select(e => Tuple.Create(e.Properties["name"].StringValue, (int?)e.Properties["skiercount"].Int64Value));
        }

        public async Task<IEnumerable<Tuple<string, DateTimeOffset, int>>> LiftWaitHistoryAsync(TimeSpan timeBack)
        {
            DateTimeOffset time = DateTimeOffset.UtcNow.Add(-timeBack);
            var query = new TableQuery { FilterString = $"PartitionKey gt '{time:o}'" };
            var segment = await _historyTable.ExecuteQuerySegmentedAsync(query, null);

            return segment.Select(e => Tuple.Create(e.RowKey, DateTimeOffset.Parse(e.PartitionKey), (int)e.Properties["skiercount"].Int64Value));
        }
    }
}
