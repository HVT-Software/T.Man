using H.Infrastructure.Database;

namespace H.Api.Common.Base;

public abstract class BaseMediatR(IServiceProvider serviceProvider) {
    protected readonly DrContext db = serviceProvider.GetRequiredService<DrContext>();
    protected readonly IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
}
