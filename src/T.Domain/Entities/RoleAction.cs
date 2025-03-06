namespace T.Domain.Entities;

public class RoleAction {
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string Key { get; set; } = string.Empty;
    public virtual Role? Role { get; set; }
}
