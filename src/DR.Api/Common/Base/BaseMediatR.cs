using DR.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DR.Api.Common.Base;

public abstract class BaseMediatR(IServiceProvider serviceProvider) {
    protected readonly DrContext db = serviceProvider.GetRequiredService<DrContext>();
    protected readonly IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
}
