#region

using Microsoft.AspNetCore.Mvc;
using FileResult = T.Domain.Common.FileResult;

#endregion

namespace T.Application.Base;

public abstract class BaseController(IServiceProvider serviceProvider) : ControllerBase
{
    protected readonly IMediator mediator = serviceProvider.GetRequiredService<IMediator>();

    protected FileContentResult File(FileResult file)
    {
        return File(file.ByteArray, "application/octet-stream", file.FileName);
    }
}
