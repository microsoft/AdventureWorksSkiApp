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

        public string AccessToken { get; private set; }
        public User User { get; private set; }

        public AuthService() {
            _httpService = HTTPService.Instance;
            AccessToken = null;
            User = null;
        }

        public async Task<bool> Login(string username, string password)
        {
            var formContent = buildLoginForm(username, password);

            var response = await _httpService.Post("/connect/token", formContent);

            LoginResponse loginResponse = new LoginResponse();

            if (response.Content != null)
                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content);

            if (loginResponse.AccessToken != null)
            {
                AccessToken = loginResponse.AccessToken;
                await fetchUserInfo();
                return true;
            }
            else
            {
                return false;
            }
        }

        private HttpContent buildLoginForm(string username, string password)
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "skiresort"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("scope", "api"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });
        }

        private async Task fetchUserInfo()
        {
            var response = await _httpService.Get("/api/users/user", AccessToken);

            User = null;

            if (response.IsSuccessful)
                User = JsonConvert.DeserializeObject<User>(response.Content);
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

    class User
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }
    }
}
