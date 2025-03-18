#region

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using T.Application.Base;
using T.Application.Queries.Auth;
using T.Domain.Extensions;
using T.Domain.Helpers;

#endregion

namespace T.Application.Commands.Auth;

internal class RegisterMerchantCommandValidator : AbstractValidator<RegisterMerchantCommand>
{
    public RegisterMerchantCommandValidator()
    {
        RuleFor(x => x.Provider).NotEmpty(o => o.Provider).MinMax(o => o.Provider, 5, 50);

        RuleFor(x => x.Username).NotEmpty(o => o.Username).MinMax(o => o.Username, 5, 255);

        RuleFor(x => x.Email).EmailAddress().When(o => !string.IsNullOrEmpty(o.Email)).WithMessage(o => o.InValid(x => x.Email));
    }
}


public class RegisterMerchantCommand : IRequest<LoginResult>
{
    public          string  Provider { get; set; } = string.Empty;
    public required string  Username { get; set; }
    public          string? Password { get; set; }
    public          string? Email    { get; set; }
    public          string? Name     { get; set; }
    public          string? Image    { get; set; }
}


public class RegisterMerchantHandler(IServiceProvider serviceProvider) : BaseHandler<RegisterMerchantCommand, LoginResult>(serviceProvider)
{
    private readonly IMediator mediator = serviceProvider.GetRequiredService<IMediator>();

    public override async Task<LoginResult> Handle(RegisterMerchantCommand request, CancellationToken cancellationToken)
    {
        User? user = await db.Users.FirstOrDefaultAsync(o => o.Username == request.Username, cancellationToken);
        if (user == null)
        {
            await using IDbContextTransaction transaction = await db.Database.BeginTransactionAsync(cancellationToken);

            Merchant merchant = await AddMerchant(request, cancellationToken);
            user = await AddUserAdmin(request, cancellationToken, merchant);

            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        return await mediator.Send(
            new LoginQuery
            {
                Username    = user.Username,
                HasPassword = user.Password,
            },
            cancellationToken);
    }

    private async Task<User> AddUserAdmin(
        RegisterMerchantCommand request,
        CancellationToken cancellationToken,
        Merchant merchant)
    {
        User user = new()
        {
            Id         = Guid.NewGuid(),
            Username   = request.Username,
            Password   = PasswordHelper.Hash(request.Password ?? StringExtension.GeneratePassword()),
            MerchantId = merchant.Id,
            IsActive   = true,
            IsAdmin    = true,
            CreatedAt  = DateTimeOffset.UtcNow,
            Email      = request.Email,
            Name       = request.Name ?? "",
            Avatar     = request.Image,
            Provider   = request.Provider,
        };

        await db.Users.AddAsync(user, cancellationToken);
        return user;
    }

    private async Task<Merchant> AddMerchant(RegisterMerchantCommand request, CancellationToken cancellationToken)
    {
        Merchant merchant = new()
        {
            Id         = Guid.Empty,
            Name       = request.Name ?? "",
            Code       = request.Username,
            SearchName = request.Username.UnsignedUnicode(),
            CreatedAt  = DateTimeOffset.UtcNow,
            IsActive   = true,
        };
        await db.Merchants.AddAsync(merchant, cancellationToken);
        return merchant;
    }
}
