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
        IQueryable<TransactionDto> query = db.Transactions.AsNoTracking()
            .Where(x => x.MerchantId == request.MerchantId && !x.IsDeleted)
            .Select(o => TransactionDto.ToDto(o));

        return new WrapperData<TransactionDto> {
            Items      = await query.Paging(request.PageIndex, request.PageSize).ToListAsync(cancellationToken),
            TotalCount = await query.CountAsync(cancellationToken),
        };
    }
}
