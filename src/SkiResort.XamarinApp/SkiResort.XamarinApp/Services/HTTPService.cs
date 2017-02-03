using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<HTTPServiceResponse> Get(string path, string bearerToken = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = getUri(path),
                Method = HttpMethod.Get
            };
            if (bearerToken != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            return await sendRequest(request);
        }

        public async Task<HTTPServiceResponse> Post(string path, string body)
        {
            return await Post(path, new StringContent(body, Encoding.UTF8, "application/json"));
        }

        public async Task<HTTPServiceResponse> Post(string path, HttpContent content)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = getUri(path),
                Method = HttpMethod.Post,
                Content = content
            };

            return await sendRequest(request);
        }

        private async Task<HTTPServiceResponse> sendRequest(HttpRequestMessage request)
        {
            HTTPServiceResponse response = null;

            try
            {
                response = await processResponseMessage(await _httpClient.SendAsync(request));
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
