using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jules02.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jules02.Core.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfiguration _apiConfig;

        public ApiClient(ApiConfiguration apiConfig)
        {
            _httpClient = new HttpClient();
            _apiConfig = apiConfig;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_apiConfig.OAuthConfig == null)
            {
                throw new InvalidOperationException("OAuth configuration is missing.");
            }

            var request = new HttpRequestMessage(HttpMethod.Post, _apiConfig.OAuthConfig.TokenUrl);

            var body = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", _apiConfig.OAuthConfig.GrantType),
                new KeyValuePair<string, string>("client_id", _apiConfig.OAuthConfig.ClientId ?? string.Empty),
                new KeyValuePair<string, string>("client_secret", _apiConfig.OAuthConfig.ClientSecret ?? string.Empty),
                new KeyValuePair<string, string>("scope", _apiConfig.OAuthConfig.Scope ?? string.Empty)
            };

            request.Content = new FormUrlEncodedContent(body);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

            if (tokenResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize token response.");
            }

            return tokenResponse.access_token;
        }

        public async Task<string> GetAsync(string endpoint)
        {
            var accessToken = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"{_apiConfig.BaseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}