using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Application.Commands.Auth;
using T.Application.Queries.Auth;
using T.Domain.Common;

namespace T.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/auth")]
public class AuthController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<Result> Login(LoginQuery req)
    {
        LoginResult res = await mediator.Send(req);
        return Result.Ok(res);
    }

    [HttpPost("{provider}")]
    [Authorize]
    public async Task<Result> Register([FromRoute] string provider, [FromBody] RegisterMerchantCommand command)
    {
        command.Provider = provider;
        LoginResult res = await mediator.Send(command);
        return Result.Ok(res);
    }
}
