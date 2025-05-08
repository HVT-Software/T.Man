using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Domain.Interfaces;

namespace T.Application.Queries.Translate;

public class DefinitionsQuery : IRequest<string> {
    [FromQuery(Name = "keyword")]
    public string Keyword { get; set; } = string.Empty;
}


internal class DefinitionsHandler(IServiceProvider serviceProvider) : BaseHandler<DefinitionsQuery, string>(serviceProvider) {
    private readonly ITranslateService translateService = serviceProvider.GetRequiredService<ITranslateService>();

    public override async Task<string> Handle(DefinitionsQuery request, CancellationToken cancellationToken) {
        return await translateService.GetDefinitionsAsync(request.Keyword, cancellationToken);
    }
}
