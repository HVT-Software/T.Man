namespace T.Domain.Interfaces;

public interface ITranslateService {
    Task<string> TranslateAsync(
        string text,
        string fromLang = "auto",
        string toLang = "vi",
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetAlternateTranslationsAsync(
        string text,
        string fromLang = "auto",
        string toLang = "vi",
        CancellationToken cancellationToken = default);

    Task<string> GetDefinitionsAsync(string keyword, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetExamplesAsync(string keyword, CancellationToken cancellationToken = default);

    Task<string> GetTransliterationAsync(string keyword, CancellationToken cancellationToken = default);

    Task<string> SuggestAsync(string text, CancellationToken cancellationToken = default);

    Task<(string Language, double Confidence)> DetectLanguageAsync(string text, CancellationToken cancellationToken = default);

    Task<string> SeeMoreAsync(string keyword, CancellationToken cancellationToken = default);
}
