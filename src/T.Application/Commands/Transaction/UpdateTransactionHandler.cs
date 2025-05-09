#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Constants;
using T.Domain.Extensions;

#endregion

namespace T.Application.Commands.Transaction;

public class UpdateTransactionCommand : UpdateRequest<TransactionDto, TransactionDto> { }


internal class UpdateTransactionHandler(IServiceProvider serviceProvider)
    : BaseHandler<UpdateTransactionCommand, TransactionDto>(serviceProvider) {
    public override async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken) {
        Domain.Entities.Transaction? entity = await db.Transactions.AsTracking()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        AppEx.ThrowIfNull(entity, Messages.NotFound);

        entity.CategoryId  = request.Model.CategoryId;
        entity.Type        = request.Model.Type;
        entity.Amount      = request.Model.Amount;
        entity.Description = request.Model.Description;
        entity.Date        = request.Model.Date;

        await db.SaveChangesAsync(cancellationToken);
        return TransactionDto.ToDto(entity);
    }
}
