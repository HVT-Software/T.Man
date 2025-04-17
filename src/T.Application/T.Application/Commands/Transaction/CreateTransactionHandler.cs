using FluentValidation;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Enums;
using T.Domain.Extensions;

namespace T.Application.Commands.Transaction;

public class CreateTransactionCommand : Request<TransactionDto> {
    public Guid             CategoryId  { get; set; }
    public ETransactionType Type        { get; set; }
    public decimal          Amount      { get; set; }
    public string           Description { get; set; } = string.Empty;
    public DateTimeOffset   Date        { get; set; }
}


internal class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand> {
    public CreateTransactionCommandValidator() {
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Amount).Min(o => o.Amount, 1);
        RuleFor(x => x.Description).Max(o => o.Description, 2000).When(o => !string.IsNullOrEmpty(o.Description));
    }
}


public class CreateTransactionHandler(IServiceProvider serviceProvider)
    : BaseHandler<CreateTransactionCommand, TransactionDto>(serviceProvider) {
    public override async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken) {
        Domain.Entities.Transaction entity = new() {
            Id          = Guid.NewGuid(),
            UserId      = request.UserId,
            MerchantId  = request.MerchantId,
            CategoryId  = request.CategoryId,
            Description = request.Description,
            Amount      = request.Amount,
            Date        = request.Date,
            CreatedAt   = DateTimeOffset.UtcNow,
            Type        = request.Type,
        };

        await db.Transactions.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return TransactionDto.ToDto(entity);
    }
}
