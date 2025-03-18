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
    : BaseHandler<ListCategoryQuery, WrapperData<CategoryDto>>(serviceProvider)
{
    public override async Task<WrapperData<CategoryDto>> Handle(ListCategoryQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Category> query = db.Categories.AsNoTracking()
            .Where(x => x.MerchantId == request.MerchantId && !x.IsDeleted)
            .Paging(!request.IsAll, request.PageIndex, request.PageSize);

        return new WrapperData<CategoryDto>
        {
            Items      = await query.Select(o => CategoryDto.ToDto(o)).ToListAsync(cancellationToken),
            TotalCount = await query.CountIf(!request.IsAll, o => o.MerchantId, cancellationToken),
        };
    }
}
