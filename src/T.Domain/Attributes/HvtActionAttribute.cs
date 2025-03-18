#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using T.Domain.Constants;
using T.Domain.Enums.Systems;
using T.Domain.Interfaces;

#endregion

namespace T.Domain.Attributes;

public class HvtActionAttribute(params EAction[] actions) : AuthorizeAttribute,
    IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user       = context.HttpContext.User;
        Claim?          tokenAdmin = user.FindFirst(TokenKey.TokenAdmin);
        if (tokenAdmin != null) { return; }

        if (!Guid.TryParse(user.FindFirst(TokenKey.TokenMerchantId)?.Value, out Guid merchantId)
         || !Guid.TryParse(user.FindFirst(TokenKey.TokenUserId)?.Value, out Guid userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        IServiceProvider serviceProvider = context.HttpContext.RequestServices;
        string           environment     = serviceProvider.GetRequiredService<IWebHostEnvironment>().EnvironmentName;
        IRedisService    redisService    = serviceProvider.GetRequiredService<IRedisService>();

        string       key         = RedisKey.GetSessionActionKey(environment, merchantId, userId);
        List<string> userActions = redisService.GetSetValue(key);

        List<string> requiredActions = GetRequiredActions();
        if (requiredActions.Count != 0 && !userActions.Any(requiredActions.Contains)) { context.Result = new ForbidResult(); }
    }

    private List<string> GetRequiredActions()
    {
        List<string> requiredActions = [];
        foreach (EAction action in actions)
        {
            FieldInfo  field      = action.GetType().GetField(action.ToString()) ?? throw new InvalidEnumArgumentException(nameof(action));
            Attribute? customAttr = GetCustomAttribute(field, typeof(ActionAttribute));
            if (customAttr is ActionAttribute actionAttr) { requiredActions.Add(actionAttr.Key); }
        }

        return requiredActions;
    }
}
