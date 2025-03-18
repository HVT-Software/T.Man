#region

using T.Domain.Common.Interfaces;

#endregion

namespace T.Domain.Entities;

public class Category : IEntity
{
    public         Guid           MerchantId  { get; set; }
    public         string         Name        { get; set; } = string.Empty;
    public         decimal        Budget      { get; set; }
    public         string?        Description { get; set; }
    public         DateTimeOffset CreateAt    { get; set; }
    public virtual Merchant?      Merchant    { get; set; }
    public         Guid           Id          { get; set; }
    public         bool           IsDeleted   { get; set; }
}
