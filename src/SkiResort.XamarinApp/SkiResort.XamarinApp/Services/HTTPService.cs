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

        private Uri getUri(string path)
        {
            return new Uri(BaseUrl + path);
        }

        public async Task<string> Get(string path)
        {
            var client = new HttpClient();
            var uri = getUri(path);

            try
            {
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
            } catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<string> Post(string path, string body)
        {
            var client = new HttpClient();
            var uri = getUri(path);

            var response = await client.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
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
