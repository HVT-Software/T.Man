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
        return Result.Ok(result);
    }

    [HttpGet("{id}")]
    [HvtAction(EAction.CategoryView)]
    public async Task<Result> GetById([FromRoute] Guid id) {
        GetCategoryByIdQuery request = new() {
            Id = id,
        };
        CategoryDto result = await mediator.Send(request);
        return Result.Ok(result);
    }

    [HttpPost]
    public async Task<Result> Create(CreateCategoryCommand command) {
        CategoryDto result = await mediator.Send(command);
        return Result.Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<Result> Update([FromRoute] Guid id, [FromBody] UpdateCategoryCommand command) {
        command.Id = id;
        CategoryDto result = await mediator.Send(command);
        return Result.Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<Result> Delete([FromRoute] Guid id) {
        await mediator.Send(
            new DeleteCategoryCommand {
                Id = id,
            });
        return Result.Ok();
    }
}
