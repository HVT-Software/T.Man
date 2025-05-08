using System.Net.Http.Json;
using System.Text.Json;
using T.Domain.Interfaces;

namespace T.Domain.Services;

// TranslateService.cs


public class TranslateService(IHttpClientFactory httpClientFactory) : ITranslateService {
    private readonly HttpClient http = httpClientFactory.CreateClient("datpmt");

    public async Task<string> TranslateAsync(
        string text,
        string fromLang = "auto",
        string toLang = "vi",
        CancellationToken cancellationToken = default) {
        Ensure(text, nameof(text));
        string? resp = await http.GetFromJsonAsync<string>(
            $"translate?string={Uri.EscapeDataString(text)}&from_lang={fromLang}&to_lang={toLang}",
            cancellationToken);
        return resp!;
    }

    public async Task<IReadOnlyList<string>> GetAlternateTranslationsAsync(
        string text,
        string fromLang = "auto",
        string toLang = "vi",
        CancellationToken cancellationToken = default) {
        Ensure(text, nameof(text));
        List<string>? resp = await http.GetFromJsonAsync<List<string>>(
            $"alternate_translations?string={Uri.EscapeDataString(text)}&from_lang={fromLang}&to_lang={toLang}",
            cancellationToken);
        return resp!;
    }

    public async Task<IReadOnlyList<string>> GetExamplesAsync(string keyword, CancellationToken cancellationToken = default) {
        Ensure(keyword, nameof(keyword));
        List<string>? resp = await http.GetFromJsonAsync<List<string>>(
            $"examples?keyword={Uri.EscapeDataString(keyword)}",
            cancellationToken);
        return resp!;
    }

    public async Task<string> GetTransliterationAsync(string keyword, CancellationToken cancellationToken = default) {
        Ensure(keyword, nameof(keyword));
        return await http.GetFromJsonAsync<string>($"transliteration?keyword={Uri.EscapeDataString(keyword)}", cancellationToken)
         ?? string.Empty;
    }

    public async Task<string> SuggestAsync(string text, CancellationToken cancellationToken = default) {
        Ensure(text, nameof(text));
        return await http.GetFromJsonAsync<string>($"suggest?string={Uri.EscapeDataString(text)}", cancellationToken) ?? string.Empty;
    }

    public async Task<(string Language, double Confidence)> DetectLanguageAsync(
        string text,
        CancellationToken cancellationToken = default) {
        Ensure(text, nameof(text));

        // API trả về mảng [lang, confidence]
        JsonElement[]? arr = await http.GetFromJsonAsync<JsonElement[]>(
            $"detection?string={Uri.EscapeDataString(text)}",
            cancellationToken);
        string lang = arr![0].GetString()!;
        double conf = arr[1].GetDouble();
        return(lang, conf);
    }

    public async Task<string> SeeMoreAsync(string keyword, CancellationToken cancellationToken = default) {
        Ensure(keyword, nameof(keyword));
        return await http.GetFromJsonAsync<string>($"see_more?keyword={Uri.EscapeDataString(keyword)}", cancellationToken) ?? string.Empty;
    }

    public async Task<string> GetDefinitionsAsync(string keyword, CancellationToken cancellationToken = default) {
        Ensure(keyword, nameof(keyword));
        string? resp = await http.GetStringAsync($"definitions?keyword={Uri.EscapeDataString(keyword)}", cancellationToken);
        return resp!;
    }

    private static void Ensure(string s, string paramName) {
        if (string.IsNullOrWhiteSpace(s)) { throw new ArgumentException("Dữ liệu không được để trống.", paramName); }
    }
}
