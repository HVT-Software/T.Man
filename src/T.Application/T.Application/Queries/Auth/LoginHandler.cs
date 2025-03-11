using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using T.Application.Base;
using T.Domain.Constants;
using T.Domain.Extensions;
using T.Domain.Helpers;
using T.Domain.Interfaces;

namespace T.Application.Queries.Auth;

public class LoginCommand : IRequest<LoginResult> {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResult {
    public string RefreshToken { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string MerchantCode { get; set; } = string.Empty;
    public string MerchantName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long ExpiredTime { get; set; }
    public long Session { get; set; }
}



public class LoginHandler(IServiceProvider serviceProvider) : BaseHandler<LoginCommand, LoginResult>(serviceProvider) {
    private readonly IRedisService redisService = serviceProvider.GetRequiredService<IRedisService>();

    public override async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken) {
        request.Username = request.Username.ToLower().Trim();

        var user = await this.db.Users
            .Include(o => o.Role)
            .Include(o => o.Merchant)
            .Where(o => o.Username == request.Username)
            .FirstOrDefaultAsync(cancellationToken);
        AppEx.ThrowIfNull(user, Messages.User_NotFound);
        AppEx.ThrowIfFalse(PasswordHelper.Verify(request.Password, user.Password), Messages.User_IncorrectPassword);

        List<string> actions = [];
        if (!user.IsAdmin) {
            actions = await this.db.RoleActions.AsNoTracking()
                .Where(o => o.RoleId == user.RoleId)
                .OrderBy(o => o.Key)
                .Select(o => o.Key)
                .ToListAsync(cancellationToken);
            AppEx.ThrowIfNullOrEmpty(actions, Messages.User_NotAllowAccess);
        }

        user.LastSession = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await this.db.SaveChangesAsync(cancellationToken);


        var claims = new List<Claim>() {
            new(Constants.TokenMerchantId, user.Merchant!.Id.ToString()),
            new(Constants.TokenUserId, user.Id.ToString()),
            new(Constants.TokenSession, user.LastSession.ToString())
        };

        if (user.IsAdmin) {
            claims.Add(new Claim(Constants.TokenAdmin, Constants.TokenAdmin));
        }

        var expiredAt = this.GetTokenExpiredAt();
        var expiredTime = new DateTimeOffset(expiredAt).ToUnixTimeMilliseconds();
        var ttlKey = TimeSpan.FromMilliseconds(expiredTime - user.LastSession);

        var sessionKey = RedisKey.GetSessionKey(environment, user.Merchant!.Id, user.Id);
        await this.redisService.SetAsync(sessionKey, user.LastSession, ttlKey);
        var actionKey = RedisKey.GetSessionActionKey(environment, user.Merchant!.Id, user.Id);
        await this.redisService.SetValueAsync(actionKey, actions, ttlKey);

        return new LoginResult {
            RefreshToken = this.GenerateRefreshToken(claims),
            Token = this.GenerateToken(claims, expiredAt),
            ExpiredTime = new DateTimeOffset(expiredAt).ToUnixTimeMilliseconds(),
            MerchantCode = user.Merchant!.Code,
            MerchantName = user.Merchant!.Name,
            Username = user.Username,
            Name = user.Name,
            Session = user.LastSession,
        };
    }

    private DateTime GetTokenExpiredAt() {
        long tokenExpiredAfterDays = this.configuration.GetSection("TokenExpire").Get<long?>() ?? 1;
        return DateTimeOffset.Now.AddDays(tokenExpiredAfterDays).Date;
    }

    private DateTime GetRefreshTokenExpiredAt() {
        int tokenExpiredAfterDays = this.configuration.GetSection("RefreshTokenExpire").Get<int?>() ?? 1;
        return DateTimeOffset.Now.AddMonths(tokenExpiredAfterDays).Date;
    }

    private string GenerateRefreshToken(List<Claim> claims) {
        var claimsForRefreshToken = new List<Claim>();
        claimsForRefreshToken.AddRange(claims);
        claimsForRefreshToken.Add(new Claim(Constants.TokenRefreshToken, "true"));

        return this.GenerateToken(claimsForRefreshToken, this.GetRefreshTokenExpiredAt());
    }

    private string GenerateToken(List<Claim> claims, DateTime expiredAt) {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JwtSecret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(claims: claims,
            expires: expiredAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
