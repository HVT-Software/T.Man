#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Enums;
using T.Domain.Extensions;

#endregion

namespace T.Application.Queries.Category;

public class ListCategoryQuery : PaginatedRequest<WrapperData<CategoryDto>> {
    public bool HasRemaining { get; set; }
}


public class ListCategoryHandler(IServiceProvider serviceProvider)
    : BaseHandler<ListCategoryQuery, WrapperData<CategoryDto>>(serviceProvider) {
    public override async Task<WrapperData<CategoryDto>> Handle(ListCategoryQuery request, CancellationToken cancellationToken) {
        IQueryable<CategoryDto> query = db.Categories.AsNoTracking()
            .Include(o => o.Transactions)
            .Where(x => x.MerchantId == request.MerchantId && !x.IsDeleted)
            .WhereDate(request.From, request.To, o => o.CreateAt)
            .OrderByDescending(o => o.CreateAt)
            .Select(o => CategoryDto.ToDto(o));

        int               count = await query.CountIf(request.IsCount, o => o.Id, cancellationToken);
        List<CategoryDto> items = await query.Paging(!request.IsAll, request.Skip, request.Take).ToListAsync(cancellationToken);


        if (request.HasRemaining) {
            (DateTimeOffset? fromMonth, DateTimeOffset? toMonth) = DateTimeOffset.UtcNow.GetMonthRange();
            var categoryIds = items.Select(o => o.Id).ToList();
            Dictionary<Guid, decimal> categoryRemaining = await db.Transactions.AsNoTracking()
                .Where(x => x.MerchantId == request.MerchantId && !x.IsDeleted)
                .Where(o => o.Type == ETransactionType.Expense && o.CategoryId.HasValue && categoryIds.Contains(o.CategoryId.Value))
                .WhereDate(fromMonth, toMonth, o => o.Date)
                .GroupBy(o => o.CategoryId!.Value)
                .ToDictionaryAsync(k => k.Key, v => v.Sum(o => o.Amount), cancellationToken);

            items.ForEach(o => o.Remaining = o.Budget - categoryRemaining.GetValueOrDefault(o.Id, 0));
        }

        return new WrapperData<CategoryDto> {
            Items      = items,
            TotalCount = count,
        };
    }
}
