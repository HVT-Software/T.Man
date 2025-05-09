#region

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using T.Application.Base;
using T.Domain.Constants;
using T.Domain.Extensions;
using T.Domain.Interfaces;

#endregion

namespace T.Application.Queries.Auth;

public class LoginQuery : IRequest<LoginResult> {
    public string Username    { get; set; } = string.Empty;
    public string Password    { get; set; } = string.Empty;
    public string HasPassword { get; set; } = string.Empty;
}


public class LoginResult {
    public string  RefreshToken { get; set; } = string.Empty;
    public string  Token        { get; set; } = string.Empty;
    public string  MerchantCode { get; set; } = string.Empty;
    public string  MerchantName { get; set; } = string.Empty;
    public string  Username     { get; set; } = string.Empty;
    public string? Name         { get; set; }
    public string? Email        { get; set; }
    public string  Image        { get; set; } = string.Empty;
    public long    ExpiredTime  { get; set; }
    public long    Session      { get; set; }
}


public class LoginHandler(IServiceProvider serviceProvider) : BaseHandler<LoginQuery, LoginResult>(serviceProvider) {
    private readonly IRedisService redisService = serviceProvider.GetRequiredService<IRedisService>();

    public override async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken) {
        request.Username = request.Username.ToLower().Trim();

        User? user = await db.Users.Include(o => o.Role)
            .Include(o => o.Merchant)
            .Where(o => o.Username == request.Username)
            .FirstOrDefaultAsync(cancellationToken);
        AppEx.ThrowIfNull(user, Messages.User_NotFound);

        // AppEx.ThrowIfFalse(
        //     PasswordHelper.Verify(request.Password, user.Password) || request.HasPassword.Equals(user.Password),
        //     Messages.User_IncorrectPassword);

        List<string> actions = [];
        if (!user.IsAdmin) {
            actions = await db.RoleActions.AsNoTracking()
                .Where(o => o.RoleId == user.RoleId)
                .OrderBy(o => o.Key)
                .Select(o => o.Key)
                .ToListAsync(cancellationToken);
            AppEx.ThrowIfNullOrEmpty(actions, Messages.User_NotAllowAccess);
        }

        user.LastSession = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await db.SaveChangesAsync(cancellationToken);

        List<Claim> claims = new() {
            new Claim(TokenKey.TokenMerchantId, user.Merchant!.Id.ToString()),
            new Claim(TokenKey.TokenUserId, user.Id.ToString()),
            new Claim(TokenKey.TokenSession, user.LastSession.ToString()),
        };

        if (user.IsAdmin) { claims.Add(new Claim(TokenKey.TokenAdmin, TokenKey.TokenAdmin)); }

        DateTime expiredAt   = GetTokenExpiredAt();
        long     expiredTime = new DateTimeOffset(expiredAt).ToUnixTimeMilliseconds();
        var      ttlKey      = TimeSpan.FromMilliseconds(expiredTime - user.LastSession);

        string sessionKey = RedisKey.GetSessionKey(environment, user.Merchant!.Id, user.Id);
        await redisService.SetAsync(sessionKey, user.LastSession, ttlKey);
        string actionKey = RedisKey.GetSessionActionKey(environment, user.Merchant!.Id, user.Id);
        await redisService.SetValueAsync(actionKey, actions, ttlKey);

        return new LoginResult {
            RefreshToken = GenerateRefreshToken(claims),
            Token        = GenerateToken(claims, expiredAt),
            ExpiredTime  = new DateTimeOffset(expiredAt).ToUnixTimeMilliseconds(),
            MerchantCode = user.Merchant!.Code,
            MerchantName = user.Merchant!.Name,
            Username     = user.Username,
            Name         = user.Name,
            Session      = user.LastSession,
            Email        = user.Email,
            Image        = user.Avatar ?? "",
        };
    }

    private DateTime GetTokenExpiredAt() {
        long tokenExpiredAfterDays = configuration.GetSection("TokenExpire").Get<long?>() ?? 1;
        return DateTimeOffset.Now.AddDays(tokenExpiredAfterDays).Date;
    }

    private DateTime GetRefreshTokenExpiredAt() {
        int tokenExpiredAfterDays = configuration.GetSection("RefreshTokenExpire").Get<int?>() ?? 1;
        return DateTimeOffset.Now.AddMonths(tokenExpiredAfterDays).Date;
    }

    private string GenerateRefreshToken(List<Claim> claims) {
        List<Claim> claimsForRefreshToken = new();
        claimsForRefreshToken.AddRange(claims);
        claimsForRefreshToken.Add(new Claim(TokenKey.TokenRefreshToken, "true"));

        return GenerateToken(claimsForRefreshToken, GetRefreshTokenExpiredAt());
    }

    private string GenerateToken(List<Claim> claims, DateTime expiredAt) {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["JwtSecret"]!));
        SigningCredentials   credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(claims: claims, expires: expiredAt, signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
