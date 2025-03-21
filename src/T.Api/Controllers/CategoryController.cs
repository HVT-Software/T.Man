#region

using Microsoft.AspNetCore.Mvc;
using T.Application.Base;
using T.Application.Commands.Category;
using T.Application.Models.Dto;
using T.Application.Queries.Category;
using T.Domain.Attributes;
using T.Domain.Common;
using T.Domain.Enums.Systems;

#endregion

namespace T.Api.Controllers;

[ApiController]
[HvtAction]
[Route("api/categories")]
public class CategoryController(IServiceProvider serviceProvider) : BaseController(serviceProvider) {
    [HttpGet]
    [HvtAction(EAction.CategoryView)]
    public async Task<Result> GetList([FromQuery] ListCategoryQuery request) {
        WrapperData<CategoryDto> result = await mediator.Send(request);
        return Result<WrapperData<CategoryDto>>.Ok(result);
    }

    [HttpGet("{id}")]
    [HvtAction(EAction.CategoryView)]
    public async Task<Result> GetById([FromRoute] Guid id) {
        var          request = new GetCategoryByIdQuery { Id = id };
        CategoryDto? result  = await mediator.Send(request);
        return Result<CategoryDto>.Ok(result);
    }

    [HttpPost]
    [HvtAction(EAction.CategoryEdit)]
    public async Task<Result> Create(CreateCategoryCommand command) {
        CategoryDto result = await mediator.Send(command);
        return Result<CategoryDto>.Ok(result);
    }

    [HttpPut("{id}")]
    [HvtAction(EAction.CategoryEdit)]
    public async Task<Result> Update([FromRoute] Guid id, [FromBody] CategoryDto model) {
        CategoryDto result = await mediator.Send(
            new UpdateCategoryCommand {
                Id    = id,
                Model = model,
            });
        return Result<CategoryDto>.Ok(result);
    }

    [HttpDelete("{id}")]
    [HvtAction(EAction.CategoryDelete)]
    public async Task<Result> Delete([FromRoute] Guid id) {
        await mediator.Send(new DeleteCategoryCommand { Id = id });
        return Result.Ok();
    }
}
