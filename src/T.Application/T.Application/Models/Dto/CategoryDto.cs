#region

using Newtonsoft.Json;

#endregion

namespace T.Application.Models.Dto;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class CategoryDto {
    public string  Name        { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Budget      { get; set; }

    public Guid            Id       { get; set; }
    public DateTimeOffset? CreateAt { get; set; }

    public static CategoryDto ToDto(Category category) {
        return new CategoryDto {
            Id          = category.Id,
            Name        = category.Name,
            Budget      = category.Budget,
            CreateAt    = category.CreateAt,
            Description = category.Description,
        };
    }

    public static CategoryDto ToDtoView(Category category) {
        return new CategoryDto {
            Id       = category.Id,
            Name     = category.Name,
            Budget   = category.Budget,
            CreateAt = category.CreateAt,
        };
    }
}
