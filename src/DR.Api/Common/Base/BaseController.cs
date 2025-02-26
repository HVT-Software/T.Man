using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.Api.Common.Base;

public abstract class BaseController(IServiceProvider serviceProvider) : ControllerBase {
    protected readonly IMediator mediator = serviceProvider.GetRequiredService<IMediator>();

    protected FileContentResult File(Domain.Common.FileResult file) {
        return File(file.ByteArray, "application/octet-stream", file.FileName);
    }
}
