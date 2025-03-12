using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Application.Queries.Auth;
using T.Domain.Common;

namespace T.Api.Controllers;

[ApiController, AllowAnonymous, Route("api/auth")]
public class AuthController(IServiceProvider serviceProvider) : BaseController(serviceProvider) {

    [HttpPost, Route("login")]
    public async Task<Result> Login(LoginQuery req) {
        var res = await this.mediator.Send(req);
        return Result<LoginResult>.Ok(res);
    }


    [HttpGet("/callback/{provider}")]
    public async Task<Result> Callback(string provider) {
        var result = await HttpContext.AuthenticateAsync();
        if (!result.Succeeded) throw new BadHttpRequestException("Lỗi ở đây");

        // Lấy thông tin user từ Claims
        var claims = result.Principal.Claims;
        IEnumerable<Claim> enumerable = claims as Claim[] ?? claims.ToArray();
        var email = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        return Result.Ok();
    }

    [HttpGet("{provider}")]
    public IActionResult Login(string provider) {
        var mapProvider = provider switch {
            "google" => "Google",
            "github" => "Github",
            "discord" => "Discord",
            _ => throw new BadHttpRequestException("Provider không hợp lệ")
        };
        var data = new AuthenticationProperties { RedirectUri = Url.Action(nameof(Callback), new { mapProvider }) };
        return Challenge(data, mapProvider);
    }
}
