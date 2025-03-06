using T.Domain.Attributes;
using T.Domain.Enums.Systems;
using T.Domain.Extensions;

namespace T.Application.Models.SystemDto;
public class ActionDto {
    public EModule Module { get; set; }
    public EAction Action { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsAction { get; set; }
    public List<EAction> Check { get; set; } = [];
    public List<EAction> Uncheck { get; set; } = [];

    public static ActionDto From(EAction action, List<EAction> check, List<EAction> uncheck, string? desctiption = null) {
        var attr = action.GetValue<ActionAttribute>();
        ArgumentNullException.ThrowIfNull(attr);
        return new ActionDto {
            Module = attr.Module,
            Action = action,
            Name = attr.Description,
            Description = desctiption ?? string.Empty,
            IsAction = false,
            Check = check,
            Uncheck = uncheck,
        };
    }
}
