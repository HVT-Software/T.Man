#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Extensions;

#endregion

namespace T.Application.Queries.Transaction;

public class ListTransactionQuery : PaginatedRequest<WrapperData<TransactionDto>> { }


public class ListTransactionHandler(IServiceProvider serviceProvider)
    : BaseHandler<ListTransactionQuery, WrapperData<TransactionDto>>(serviceProvider) {
    public override async Task<WrapperData<TransactionDto>> Handle(ListTransactionQuery request, CancellationToken cancellationToken) {
        IQueryable<TransactionDto> query = db.Transactions.Include(o => o.Category)
            .Where(x => x.MerchantId == request.MerchantId && !x.IsDeleted)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => TransactionDto.ToDto(o));

        return new WrapperData<TransactionDto> {
            Items      = await query.Paging(request.Skip, request.Take).ToListAsync(cancellationToken),
            TotalCount = await query.CountAsync(cancellationToken),
        };
    }
}
