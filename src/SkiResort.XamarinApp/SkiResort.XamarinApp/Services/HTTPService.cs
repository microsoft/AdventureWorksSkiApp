using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Services
{
    class HTTPService
    {
        public readonly string BaseUrl;

        public HTTPService(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public async Task<string> Get(string path)
        {
            var client = new HttpClient();
            var uri = new Uri(BaseUrl + path);

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
