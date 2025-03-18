#region

using T.Domain.Enums.Systems;
using T.Domain.Extensions;

#endregion

namespace T.Application.Models.SystemDto;

public class ModuleDto
{
    public EModule         Module          { get; set; }
    public string          Name            { get; set; } = string.Empty;
    public int             NumberOfActions { get; set; }
    public List<ModuleDto> Children        { get; set; } = [];

    public static ModuleDto From(EModule module)
    {
        return new ModuleDto
        {
            Module = module,
            Name   = module.Description(),
        };
    }
}
