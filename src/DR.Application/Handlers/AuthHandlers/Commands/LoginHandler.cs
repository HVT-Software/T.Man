using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DR.Domain.Common.Hashers;
using DR.Domain.Constants;
using DR.Domain.Extentions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DR.Application.Handlers.AuthHandlers.Commands;

public class LoginCommand : IRequest<LoginResult> {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResult {
    public string RefreshToken { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long ExpiredTime { get; set; }
    public long Session { get; set; }
}

public class LoginHandler(IServiceProvider serviceProvider) : BaseHandler<LoginCommand, LoginResult>(serviceProvider) {
    //private readonly IRedisService redisCacheService = serviceProvider.GetRequiredService<IRedisService>();

    public override async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken) {
        request.Username = request.Username.ToLower().Trim();
        return await this.Login(request, cancellationToken);
    }

    public async Task<LoginResult> Login(LoginCommand request, CancellationToken cancellationToken) {
        var user = await this.db.Users.FirstOrDefaultAsync(o => o.Username == request.Username, cancellationToken);
        DrException.ThrowIfNull(user, Messages.User_NotFound);
        DrException.ThrowIf(!user.IsAdmin && !user.IsActive, Messages.User_Inactive);
        DrException.ThrowIf(!PasswordHasher.Verify(request.Password, user.Password), Messages.User_IncorrectPassword);

        var permissions = await this.db.GetPermissions(cancellationToken: cancellationToken);
        Role? role = null;
        if (user.RoleId != null) {
            role = await this.db.Roles.Include(o => o.RolePermissions).FirstOrDefaultAsync(o => o.Id == user.RoleId, cancellationToken);
        }

        var userPermissions = UserPermissionDto.MapFromEntities(permissions, role?.RolePermissions?.ToList(), user.IsAdmin);
        DrException.ThrowIfFalse(userPermissions.Exists(o => o.IsEnable), Messages.User_NoPermission);

        var claims = new List<Claim>() {
            new(Constants.TokenUserId, user.Id.ToString()),
        };

        if (user.IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        var permissionClaims = GetClaimPermissions(userPermissions);
        claims.AddRange(permissionClaims);
        var expiredAt = this.GetTokenExpiredAt();

        return new() {
            RefreshToken = this.GenerateRefreshToken(claims),
            Token = this.GenerateToken(claims, expiredAt),
            ExpiredTime = new DateTimeOffset(expiredAt).ToUnixTimeMilliseconds(),
            Username = user.Username,
            Name = user.Name,
        };
    }

    private DateTime GetTokenExpiredAt() {
        long tokenExpiredAfterDays = this.configuration.GetSection("TokenExpire").Get<long?>() ?? 1;
        return DateTimeOffset.Now.AddDays(tokenExpiredAfterDays).Date;
    }

    private string GenerateRefreshToken(List<Claim> claims) {
        var claimsForRefreshToken = new List<Claim>();
        claimsForRefreshToken.AddRange(claims);
        claimsForRefreshToken.Add(new Claim(Constants.TokenRefreshToken, "true"));

        return this.GenerateToken(claimsForRefreshToken, DateTime.Now.AddYears(10));
    }

    private string GenerateToken(List<Claim> claims, DateTime expiredAt) {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JwtSecret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          claims: claims,
          expires: expiredAt,
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static List<Claim> GetClaimPermissions(List<UserPermissionDto> permissions) {
        List<Claim> claims = [];
        foreach (var item in permissions) {
            if (!item.IsEnable) continue;
            if (item.IsClaim)
                claims.Add(new Claim(ClaimTypes.Role, item.ClaimName));

            if (item.Items != null && item.Items.Count != 0)
                claims.AddRange(GetClaimPermissions(item.Items));
        }
        return claims;
    }
}
