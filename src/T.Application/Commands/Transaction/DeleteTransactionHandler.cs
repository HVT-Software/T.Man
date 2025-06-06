#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Domain.Common;
using T.Domain.Constants;
using T.Domain.Exceptions;

#endregion

namespace T.Application.Commands.Transaction;

public class DeleteTransactionCommand : SingleRequest { }


public class DeleteTransactionHandler(IServiceProvider serviceProvider) : BaseHandler<DeleteTransactionCommand>(serviceProvider) {
    public override async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken) {
        Domain.Entities.Transaction? entity = await db.Transactions.AsTracking()
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        AppEx.ThrowIfNull(entity, Messages.Transaction_NotFound);

        entity.IsDeleted = true;
        await db.SaveChangesAsync(cancellationToken);
    }
}
