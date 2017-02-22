

using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SkiResort.API.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        [HttpGet("{reportId}")]
        public async Task<string> GetTokenAsync(string reportId)
        {
            string name = string.Empty;
            string workspaceId = string.Empty;
            string accessKey = string.Empty;

            using (var _httpClient = new HttpClient())
            {
                string uri = $"https://api.powerbi.com/v1.0/collections/{name}/workspaces/{workspaceId}/reports";
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/query+json"));

                _httpClient.DefaultRequestHeaders.Remove("authorization");
                _httpClient.DefaultRequestHeaders.Add("authorization", $"AppKey {accessKey}");

                var response = await _httpClient.GetAsync(uri);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

