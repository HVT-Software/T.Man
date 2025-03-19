using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Application.Commands.Transaction;
using T.Application.Models.Dto;
using T.Application.Queries.Transaction;
using T.Domain.Attributes;
using T.Domain.Common;
using T.Domain.Enums.Systems;

namespace T.Api.Controllers;

[ApiController]
[HvtAction]
[Route("api/transactions")]
public class TransactionController(IServiceProvider serviceProvider) : BaseController(serviceProvider) {
    [HttpGet]
    [HvtAction(EAction.TransactionView)]
    public async Task<Result> GetList([FromQuery] ListTransactionQuery request) {
        WrapperData<TransactionDto> result = await mediator.Send(request);
        return Result.Ok(result);
    }

    [HttpPost]
    [HvtAction(EAction.TransactionEdit)]
    public async Task<Result> Create(CreateTransactionCommand command) {
        TransactionDto result = await mediator.Send(command);
        return Result.Ok(result);
    }

    [HttpPut("{id}")]
    [HvtAction(EAction.TransactionEdit)]
    public async Task<Result> Update([FromRoute] Guid id, [FromBody] TransactionDto model) {
        TransactionDto result = await mediator.Send(new UpdateTransactionCommand { Id = id, Model = model });
        return Result.Ok(result);
    }

    [HttpDelete("{id}")]
    [HvtAction(EAction.TransactionDelete)]
    public async Task<Result> Delete([FromRoute] Guid id) {
        await mediator.Send(new DeleteTransactionCommand { Id = id });
        return Result.Ok();
    }
}
