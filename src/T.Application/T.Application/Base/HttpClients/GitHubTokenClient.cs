using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace T.Application.Base.HttpClients {


    public class GitHubTokenClient : IDisposable {
        private readonly HttpClient httpClient;
        private readonly string clientId;
        private readonly string clientSecret;

        public GitHubTokenClient(string clientId, string clientSecret) {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.github.com/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
        }

        public async Task<bool> VerifyAccessTokenAsync(string accessToken) {
            // Tạo URL với Client ID
            string url = $"applications/{clientId}/token";

            // Tạo nội dung JSON cho body
            var requestBody = new {
                access_token = accessToken
            };
            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Thiết lập Basic Authentication
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

            // Gửi yêu cầu POST
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            // Xử lý phản hồi
            return response.IsSuccessStatusCode;
        }

        // Giải phóng tài nguyên
        public void Dispose() {
            httpClient.Dispose();
        }
    }

}
