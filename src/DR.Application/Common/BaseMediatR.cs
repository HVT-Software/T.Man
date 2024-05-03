using DR.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DR.Application.Common;

public abstract class BaseMediatR(IServiceProvider serviceProvider) {
    protected readonly DrContext db = serviceProvider.GetRequiredService<DrContext>();
    protected readonly IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
}
