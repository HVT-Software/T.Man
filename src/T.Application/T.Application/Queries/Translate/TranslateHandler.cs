using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Domain.Interfaces;

namespace T.Application.Queries.Translate;

public class TranslateQuery : IRequest<string> {
    [FromQuery(Name = "text")]
    public string Text { get; set; } = string.Empty;

    [FromQuery(Name = "fromLang")]
    public string FromLang { get; set; } = "auto";

    [FromQuery(Name = "toLang")]
    public string ToLang { get; set; } = "vi";
}


public class TranslateHandler(IServiceProvider serviceProvider) : BaseHandler<TranslateQuery, string>(serviceProvider) {
    private readonly ITranslateService translateService = serviceProvider.GetRequiredService<ITranslateService>();

    public override Task<string> Handle(TranslateQuery request, CancellationToken cancellationToken) {
        return translateService.TranslateAsync(
            request.Text,
            request.FromLang,
            request.ToLang,
            cancellationToken);
    }
}
