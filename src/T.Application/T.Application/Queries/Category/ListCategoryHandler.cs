#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Extensions;

#endregion

namespace T.Application.Queries.Category;

public class ListCategoryQuery : PaginatedRequest<WrapperData<CategoryDto>> { }


public class ListCategoryHandler(IServiceProvider serviceProvider)
    : BaseHandler<ListCategoryQuery, WrapperData<CategoryDto>>(serviceProvider) {
    public override async Task<WrapperData<CategoryDto>> Handle(ListCategoryQuery request, CancellationToken cancellationToken) {
        IQueryable<CategoryDto> query = db.Categories.AsNoTracking()
            .Where(x => x.MerchantId == request.MerchantId && !x.IsDeleted)
            .WhereDate(request.From, request.To, o => o.CreateAt)
            .OrderByDescending(o => o.CreateAt)
            .Select(o => CategoryDto.ToDto(o));

        return new WrapperData<CategoryDto> {
            Items      = await query.Paging(!request.IsAll, request.Skip, request.Take).ToListAsync(cancellationToken),
            TotalCount = await query.CountIf(request.IsCount, o => o.Id, cancellationToken),
        };
    }
}
