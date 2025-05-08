#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Enums;
using T.Domain.Extensions;

#endregion

namespace T.Application.Queries.Transaction;

public class ListTransactionQuery : PaginatedRequest<WrapperData<TransactionDto>> {
    public List<Guid>?             CategoryIds { get; set; }
    public List<ETransactionType>? Types       { get; set; }
}


public class ListTransactionHandler(IServiceProvider serviceProvider)
    : BaseHandler<ListTransactionQuery, WrapperData<TransactionDto>>(serviceProvider) {
    public override async Task<WrapperData<TransactionDto>> Handle(ListTransactionQuery request, CancellationToken cancellationToken) {
        request.Types       ??= [];
        request.CategoryIds ??= [];

        IQueryable<TransactionDto> query = db.Transactions.Include(o => o.Category)
            .Where(x => x.MerchantId == request.MerchantId && !x.IsDeleted)
            .WhereDate(request.From, request.To, o => o.Date)
            .WhereIf(
                !string.IsNullOrWhiteSpace(request.SearchText),
                o => o.Description.Contains(request.SearchText!) || o.Category != null && o.Category.Name.Contains(request.SearchText!))
            .WhereIf(request.CategoryIds.Count > 0, o => request.CategoryIds.Contains(o.CategoryId))
            .WhereIf(request.Types.Count > 0, o => request.Types.Contains(o.Type))
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => TransactionDto.ToDto(o));

        return new WrapperData<TransactionDto> {
            Items      = await query.Paging(request.Skip, request.Take).ToListAsync(cancellationToken),
            TotalCount = await query.CountAsync(cancellationToken),
        };
    }
}
