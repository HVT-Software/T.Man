#region

using T.Domain.Enums;

#endregion

namespace T.Application.Models.Dto;

public class TransactionDto {
    public Guid             UserId      { get; set; }
    public Guid             CategoryId  { get; set; }
    public ETransactionType Type        { get; set; }
    public decimal          Amount      { get; set; }
    public string           Description { get; set; } = string.Empty;
    public DateTimeOffset   Date        { get; set; }
    public DateTimeOffset   CreatedAt   { get; set; }
    public CategoryDto?     Category    { get; set; }

    public static TransactionDto ToDto(Transaction transaction) {
        return new TransactionDto {
            UserId      = transaction.UserId,
            CategoryId  = transaction.CategoryId,
            Type        = transaction.Type,
            Amount      = transaction.Amount,
            Description = transaction.Description,
            Date        = transaction.Date,
            CreatedAt   = transaction.CreatedAt,
            Category    = transaction.Category != null ? CategoryDto.ToDtoView(transaction.Category) : null,
        };
    }
}
