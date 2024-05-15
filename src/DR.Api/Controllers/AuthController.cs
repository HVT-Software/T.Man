using DR.Application.Handlers.AuthHandlers.Commands;
using DR.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DR.Api.Controllers;

[ApiController, AllowAnonymous, Route("api/auth")]
public class AuthController(IServiceProvider serviceProvider) : BaseController(serviceProvider) {
    [HttpPost, Route("login")]
    public async Task<Result> Login(LoginCommand req) {
        var res = await this.mediator.Send(req);
        return Result<LoginResult>.Ok(res);
    }
}
