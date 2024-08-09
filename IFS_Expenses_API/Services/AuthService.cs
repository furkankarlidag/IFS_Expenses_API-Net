using Newtonsoft.Json.Linq;

namespace IFS_Expenses_API.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetBearerToken()
        {
            var tokenUrl = "https://pame8xi-dev1.build.ifs.cloud/auth/realms/pame8xidev1/protocol/openid-connect/token"; 
            var payload = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", "NEXT_FRK"),
            new KeyValuePair<string, string>("client_secret", "yEm3zJVK4ZN1IwOK4azqVgKs8JstUuhb")

        });

            var response = await _httpClient.PostAsync(tokenUrl, payload);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();

            dynamic obj = JObject.Parse(responseData);
            return obj.access_token;
        }
    }

}
