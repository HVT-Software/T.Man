#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;

#endregion

namespace T.Application.Queries.Transaction;

public class GetTransactionByIdQuery : SingleRequest<TransactionDto?> { }


public class GetTransactionByIdHandler(IServiceProvider serviceProvider)
    : BaseHandler<GetTransactionByIdQuery, TransactionDto?>(serviceProvider) {
    public override async Task<TransactionDto?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken) {
        return await db.Transactions.Include(o => o.Category)
            .Where(o => o.MerchantId == request.MerchantId && o.Id == request.Id && !o.IsDeleted)
            .Select(o => TransactionDto.ToDto(o))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
