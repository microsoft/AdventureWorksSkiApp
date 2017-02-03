using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SkiResort.XamarinApp.Services
{
    class AuthService
    {
        #region Singleton
        private static AuthService instance;
        public static AuthService Instance
        {
            get
            {
                if (instance == null)
                    instance = new AuthService();
                return instance;
            }
        }
        #endregion

        private HTTPService _httpService;

        public AuthService() {
            _httpService = HTTPService.Instance;
        }

        public async Task<bool> Login(string username, string password)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "skiresort"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("scope", "api"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            var result = await _httpService.Post("/connect/token", formContent);

            LoginResponse loginResponse = new LoginResponse();

            if (result.Content != null)
                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(result.Content);

            if (loginResponse.Error == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class LoginResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
