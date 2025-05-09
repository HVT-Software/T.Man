#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Domain.Common;
using T.Domain.Constants;
using T.Domain.Exceptions;

#endregion

namespace T.Application.Commands.Category;

public class DeleteCategoryCommand : SingleRequest { }


public class DeleteCategoryHandler(IServiceProvider serviceProvider) : BaseHandler<DeleteCategoryCommand>(serviceProvider) {
    public override async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken) {
        Domain.Entities.Category? entity = await db.Categories.AsTracking().FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        AppEx.ThrowIfNull(entity, Messages.Category_NotFound);

        bool exists = await db.Transactions.AnyAsync(o => o.CategoryId == request.Id && !o.IsDeleted, cancellationToken);
        AppEx.ThrowIf(exists, Messages.Category_Used);

        entity.IsDeleted = true;
        await db.SaveChangesAsync(cancellationToken);
    }
}
