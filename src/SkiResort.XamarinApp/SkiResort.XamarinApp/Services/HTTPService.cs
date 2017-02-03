using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<HTTPServiceResponse> Get(string path)
        {
            var uri = getUri(path);
            HTTPServiceResponse response = null;

            try
            {
                response = await processResponseMessage(await _httpClient.GetAsync(uri));
            }
            catch
            {
                response = buildExceptionResponse();
            }

            return response;
        }

        public async Task<HTTPServiceResponse> Post(string path, string body)
        {
            return await Post(path, new StringContent(body, Encoding.UTF8, "application/json"));
        }

        public async Task<HTTPServiceResponse> Post(string path, HttpContent content)
        {
            var uri = getUri(path);
            HTTPServiceResponse response = null;

            try
            {
                response = await processResponseMessage(await _httpClient.PostAsync(uri, content));
            }
            catch
            {
                response = buildExceptionResponse();
            }

            return response;
        }

        private async Task<HTTPServiceResponse> processResponseMessage(HttpResponseMessage message)
        {
            var content = await message.Content.ReadAsStringAsync();
            if (content == "")
                content = null;
            return new HTTPServiceResponse()
            {
                IsSuccessful = message.IsSuccessStatusCode,
                StatusCode = message.StatusCode,
                Content = content
            };
        }

        private HTTPServiceResponse buildExceptionResponse()
        {
            return new HTTPServiceResponse()
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Content = null
            };
        }
    }

    public class HTTPServiceResponse
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}
