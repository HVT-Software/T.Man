using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Domain.Interfaces;

namespace T.Application.Queries.Translate;

public class AlternateQuery : IRequest<IReadOnlyList<string>> {
    [FromQuery(Name = "text")]
    public string Text { get; set; } = string.Empty;

    [FromQuery(Name = "fromLang")]
    public string FromLang { get; set; } = "auto";

    [FromQuery(Name = "toLang")]
    public string ToLang { get; set; } = "vi";
}


public class AlternateHandler(IServiceProvider serviceProvider) : BaseHandler<AlternateQuery, IReadOnlyList<string>>(serviceProvider) {
    private readonly ITranslateService translateService = serviceProvider.GetRequiredService<ITranslateService>();

    public override async Task<IReadOnlyList<string>> Handle(AlternateQuery request, CancellationToken cancellationToken) {
        return await translateService.GetAlternateTranslationsAsync(
            request.Text,
            request.FromLang,
            request.ToLang,
            cancellationToken);
    }
}
