#region

using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Application.Commands.Reminder;
using T.Domain.Attributes;
using T.Domain.Common;

#endregion

namespace T.Api.Controllers;

[ApiController]
[HvtAction]
[Route("api/reminders")]
public class ReminderController(IServiceProvider serviceProvider) : BaseController(serviceProvider) {
    [HttpPost]
    public async Task<Result> Create(CreateReminderCommand command) {
        Guid result = await mediator.Send(command);
        return Result<Guid>.Ok(result);
    }
}
