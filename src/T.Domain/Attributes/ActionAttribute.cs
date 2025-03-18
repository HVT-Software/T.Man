#region

using T.Domain.Enums.Systems;

#endregion

namespace T.Domain.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ActionAttribute(
    EModule parent,
    string description,
    string permissionKey) : Attribute
{
    public EModule Module      { get; set; } = parent;
    public string  Description { get; set; } = description;
    public string  Key         { get; set; } = permissionKey;
}
