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
        #region Singleton
        private static HTTPService instance;
        public static HTTPService Instance
        {
            get
            {
                if (instance == null)
                    instance = new HTTPService(Config.API_URL);
                return instance;
            }
        }
        #endregion

        HttpClient _httpClient;
        public readonly string BaseUrl;

        public HTTPService(string baseUrl)
        {
            _httpClient = new HttpClient();

            BaseUrl = baseUrl;
        }

        private Uri getUri(string path)
        {
            return new Uri(BaseUrl + path);
        }

        public async Task<string> Get(string path)
        {
            var uri = getUri(path);

            var response = await _httpClient.GetAsync(uri);
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

        public async Task<string> Post(string path, string body)
        {
            var uri = getUri(path);

            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
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
