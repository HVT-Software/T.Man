#region

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

#endregion

namespace T.Application.Base.HttpClients;

public class GitHubTokenClient : IDisposable {
    private readonly string     clientId;
    private readonly string     clientSecret;
    private readonly HttpClient httpClient;

    public GitHubTokenClient(string clientId, string clientSecret) {
        this.clientId          = clientId;
        this.clientSecret      = clientSecret;
        httpClient             = new HttpClient();
        httpClient.BaseAddress = new Uri("https://api.github.com/");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
    }

    // Giải phóng tài nguyên
    public void Dispose() {
        httpClient.Dispose();
    }

    public async Task<bool> VerifyAccessTokenAsync(string accessToken) {
        // Tạo URL với Client ID
        var url = $"applications/{clientId}/token";

        // Tạo nội dung JSON cho body
        var requestBody = new {
            access_token = accessToken,
        };
        string        jsonContent = JsonSerializer.Serialize(requestBody);
        StringContent content     = new(jsonContent, Encoding.UTF8, "application/json");

        // Thiết lập Basic Authentication
        string authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

        // Gửi yêu cầu POST
        HttpResponseMessage response = await httpClient.PostAsync(url, content);

        // Xử lý phản hồi
        return response.IsSuccessStatusCode;
    }
}
