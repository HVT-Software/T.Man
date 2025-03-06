using Microsoft.Extensions.Configuration;
using T.Infrastructure.Database;

namespace T.Application.Base;

public abstract class BaseMediatR(IServiceProvider serviceProvider) {
    protected readonly HvtContext db = serviceProvider.GetRequiredService<HvtContext>();
    protected readonly IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
}
