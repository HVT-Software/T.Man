using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using T.Application.Base;
using T.Application.Queries.Translate;
using T.Domain.Attributes;
using T.Domain.Common;

namespace T.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/translate")]
public class TranslateController(IServiceProvider serviceProvider) : BaseController(serviceProvider) {
    [HttpGet]
    [HvtAction]
    public async Task<Result<string>> Translate([FromQuery] TranslateQuery req) {
        string result = await mediator.Send(req);
        return Result<string>.Ok(result);
    }

    [HttpGet("alternate")]
    [HvtAction]
    public async Task<Result<IReadOnlyList<string>>> Alternate([FromQuery] AlternateQuery req) {
        IReadOnlyList<string> result = await mediator.Send(req);
        return Result<IReadOnlyList<string>>.Ok(result);
    }

    [HttpGet("definitions")]
    [HvtAction]
    public async Task<Result<object>> Definitions([FromQuery] DefinitionsQuery req) {
        string result = await mediator.Send(req);
        return Result<object>.Ok(JsonConvert.DeserializeObject(result));
    }
}
