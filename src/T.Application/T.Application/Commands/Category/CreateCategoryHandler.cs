#region

using FluentValidation;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Extensions;

#endregion

namespace T.Application.Commands.Category;

public class CreateCategoryCommand : Request<CategoryDto>
{
    public required string  Name        { get; set; }
    public          string? Description { get; set; }
    public          decimal Budget      { get; set; }
}


internal class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty(o => o.Name).MinMax(o => o.Name, 5, 255);
        RuleFor(x => x.Budget).MinMax(o => o.Budget, 0);
    }
}


public class CreateCategoryHandler(IServiceProvider serviceProvider) : BaseHandler<CreateCategoryCommand, CategoryDto>(serviceProvider)
{
    public override async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Category entity = new()
        {
            Id          = Guid.NewGuid(),
            Name        = request.Name,
            Description = request.Description,
            Budget      = request.Budget,
        };

        db.Categories.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
        return CategoryDto.ToDto(entity);
    }
}
