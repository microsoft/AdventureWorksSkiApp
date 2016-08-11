
namespace AdventureWorks.SkiResort.Infrastructure.AzureSearch
{
    public class SearchServiceConfig
    {
        public bool UseAzureSearch { get; set; }
        public string ServiceName { get; set; }
        public string ApiKey { get; set; }
        public string Indexer { get; set; }
        public string Suggester { get; set; }
    }
}
