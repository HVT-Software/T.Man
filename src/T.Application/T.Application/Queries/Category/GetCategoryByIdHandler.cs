#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;

#endregion

namespace T.Application.Queries.Category;

public class GetCategoryByIdQuery : SingleRequest<CategoryDto> { }


public class GetCategoryByIdHandler(IServiceProvider serviceProvider) : BaseHandler<GetCategoryByIdQuery, CategoryDto>(serviceProvider) {
    public override async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken) {
        return await db.Categories.AsNoTracking()
            .Where(o => o.MerchantId == request.MerchantId && !o.IsDeleted)
            .Select(o => CategoryDto.ToDto(o))
            .SingleAsync(c => c.Id == request.Id, cancellationToken);
    }
}
