using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T.Api.Common.Base;
using T.Api.Handlers.AuthHandlers.Commands;
using T.Domain.Common;

namespace T.Api.Controllers;

[ApiController, AllowAnonymous, Route("api/auth")]
public class AuthController(IServiceProvider serviceProvider) : BaseController(serviceProvider) {
    [HttpPost, Route("login")]
    public async Task<Result> Login(LoginCommand req) {
        var res = await this.mediator.Send(req);
        return Result<LoginResult>.Ok(res);
    }
}
