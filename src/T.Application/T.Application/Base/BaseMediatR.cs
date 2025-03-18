#region

using Microsoft.Extensions.Configuration;
using T.Infrastructure.Database;

#endregion

namespace T.Application.Base;

public abstract class BaseMediatR(IServiceProvider serviceProvider)
{
    protected readonly IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    protected readonly HvtContext     db            = serviceProvider.GetRequiredService<HvtContext>();
}
