using FluentValidation;
using T.Application.Base;
using T.Domain.Entities;
using T.Domain.Extensions;
using T.Domain.Helpers;

namespace T.Application.Commands.Auth {

    public class RegisterMerchantCommand : IRequest<Guid> {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
    }

    internal class RegisterMerchantCommandValidator : AbstractValidator<RegisterMerchantCommand> {
        public RegisterMerchantCommandValidator() {
            RuleFor(x => x.Username)
                .NotEmpty(o => o.Username)
                .MinMax(o => o.Username, 5, 255);

            RuleFor(x => x.Password)
                .NotEmpty(o => o.Password)
                .Phone();

            RuleFor(x => x.Email)
                .EmailAddress().When(o => !string.IsNullOrEmpty(o.Email)).WithMessage(o => o.InValid(x => x.Email));
        }
    }


    public class RegisterMerchantHandler(IServiceProvider serviceProvider) : BaseHandler<RegisterMerchantCommand, Guid>(serviceProvider) {
        public override async Task<Guid> Handle(RegisterMerchantCommand request, CancellationToken cancellationToken) {
            await using var transaction = await this.db.Database.BeginTransactionAsync(cancellationToken);

            var merchant = await AddMerchant(request, cancellationToken);
            await AddUserAdmin(request, cancellationToken, merchant);

            await this.db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return merchant.Id;
        }

        private async Task AddUserAdmin(RegisterMerchantCommand request, CancellationToken cancellationToken, Merchant merchant) {
            var user = new User() {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = PasswordHelper.Hash(request.Password),
                MerchantId = merchant.Id,
                IsActive = true,
                IsAdmin = true,
                CreatedAt = DateTimeOffset.UtcNow,
                Email = request.Email,
                Name = request.Name ?? "",
                Avatar = request.Image,
            };
            await this.db.Users.AddAsync(user, cancellationToken);
        }

        private async Task<Merchant> AddMerchant(RegisterMerchantCommand request, CancellationToken cancellationToken) {
            var merchant = new Merchant {
                Id = Guid.Empty,
                Name = request.Name ?? "",
                Code = request.Username,
                SearchName = request.Username.UnsignedUnicode(),
                CreatedAt = DateTimeOffset.UtcNow,
                IsActive = true,
            };
            await this.db.Merchants.AddAsync(merchant, cancellationToken);
            return merchant;
        }
    }
}
