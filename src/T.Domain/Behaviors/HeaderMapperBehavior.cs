#region

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using T.Domain.Common;
using T.Domain.Constants;

#endregion

namespace T.Domain.Behaviors;

public class HeaderMapperBehavior
    <TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse> where TRequest : Request {
    private readonly HttpContext? httpContext = httpContextAccessor.HttpContext;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) {
        IHeaderDictionary? headers = httpContextAccessor.HttpContext?.Request.Headers;

        if (headers != null) {
            request.UserId     = GetToken(TokenKey.TokenUserId);
            request.MerchantId = GetToken(TokenKey.TokenMerchantId);
        }

        return await next();
    }

    private Guid GetToken(string key) {
        string? stringValue = httpContext?.User.FindFirst(o => o.Type == key)?.Value ?? string.Empty;
        return Guid.TryParse(stringValue, out Guid guid) ? guid : Guid.Empty;
    }

    private Guid GetHeader(string key) {
        if (httpContext?.Request != null
         && httpContext.Request.Headers.TryGetValue(key, out StringValues value)
         && Guid.TryParse(value, out Guid guid)) { return guid; }

        return Guid.Empty;
    }
}
