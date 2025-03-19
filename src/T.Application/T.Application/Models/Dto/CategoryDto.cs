#region

using T.Domain.Common.Interfaces;

#endregion

namespace T.Application.Models.Dto;

public class CategoryDto : IEntity {
    public string  Name        { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Budget      { get; set; }
    public Guid    Id          { get; set; }

    public bool IsDeleted { get; set; }

    public static CategoryDto ToDto(Category category) {
        return new CategoryDto {
            Id          = category.Id,
            Name        = category.Name,
            Description = category.Description,
            Budget      = category.Budget,
        };
    }
}
