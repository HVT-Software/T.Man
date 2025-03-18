namespace T.Domain.Common.Interfaces;

public interface IEntity
{
    public Guid Id        { get; set; }
    public bool IsDeleted { get; set; }
}
