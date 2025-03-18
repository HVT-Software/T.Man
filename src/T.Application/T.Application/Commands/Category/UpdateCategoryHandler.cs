#region

using Microsoft.EntityFrameworkCore;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Constants;
using T.Domain.Extensions;

#endregion

namespace T.Application.Commands.Category;

public class UpdateCategoryCommand : UpdateRequest<CategoryDto, CategoryDto> { }


internal class UpdateCategoryHandler(IServiceProvider serviceProvider) : BaseHandler<UpdateCategoryCommand, CategoryDto>(serviceProvider)
{
    public override async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Category? entity = await db.Categories.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        AppEx.ThrowIfNull(entity, Messages.NotFound);

        entity.Name        = request.Model.Name;
        entity.Description = request.Model.Description;
        entity.Budget      = request.Model.Budget;

        await db.SaveChangesAsync(cancellationToken);
        return CategoryDto.ToDto(entity);
    }
}
