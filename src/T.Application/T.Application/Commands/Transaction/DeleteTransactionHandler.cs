#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Domain.Common;
using T.Domain.Constants;
using T.Domain.Extensions;

#endregion

namespace T.Application.Commands.Transaction;

public class DeleteTransactionCommand : SingleRequest { }

public class DeleteTransactionHandler(IServiceProvider serviceProvider) : BaseHandler<DeleteTransactionCommand>(serviceProvider)
{
    public override async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Transaction? entity = await db.Transactions.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        AppEx.ThrowIfNull(entity, Messages.Transaction_NotFound);

        db.Transactions.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
    }
}
