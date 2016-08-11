using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.SkiResort.Infrastructure.Infraestructure
{
    public class AzureSearchDataInitializer
    {
        private readonly string _serviceName = string.Empty;
        private readonly string _apiKey = string.Empty;
        private readonly string _indexer = string.Empty;

        public AzureSearchDataInitializer(IConfigurationRoot configuration)
        {
            _serviceName = configuration["SearchConfig:ServiceName"];
            _apiKey = configuration["SearchConfig:ApiKey"];
            _indexer = configuration["SearchConfig:Indexer"];
        }

        public async Task Initialize()
        {
            if (!await ExistsIndex())
            {
                await CreateIndex();
                await ImportData();
            }
        }

        async Task<bool> ExistsIndex()
        {
            string uri = $"https://{_serviceName}.search.windows.net/indexes/{_indexer }?api-version=2015-02-28";
            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
                var response = await _httpClient.GetAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
            }

            return false;
        }

        async Task CreateIndex()
        {
            string uri = $"https://{_serviceName}.search.windows.net/indexes?api-version=2015-02-28";

            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

                string file = File.ReadAllText("azureSearch/index.json");
                await _httpClient.PostAsync(uri, new StringContent(file, Encoding.UTF8, "application/json"));
            }
        }

        async Task ImportData()
        {
            string uri = $"https://{_serviceName}.search.windows.net/indexes/{_indexer }/docs/index?api-version=2015-02-28";

            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

                string file = File.ReadAllText("azureSearch/data.json");
                await _httpClient.PostAsync(uri, new StringContent(file, Encoding.UTF8, "application/json"));
            }
        }

    }


}
