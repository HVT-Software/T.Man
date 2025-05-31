#region

using FluentValidation;
using T.Application.Base;
using T.Application.Models.Dto;
using T.Domain.Common;
using T.Domain.Extensions;

#endregion

namespace T.Application.Commands.Reminder;

public class CreateReminderCommand : UpdateRequest<ReminderDto, Guid> { }


internal class CreateReminderCommandValidator : AbstractValidator<CreateReminderCommand> {
    public CreateReminderCommandValidator() {
        RuleFor(x => x.Model.Title).NotEmpty(o => o.Model.Title).MinMax(o => o.Model.Title, 5, 255);
        RuleFor(x => x.Model.CreatedAt).GreaterThan(DateTimeOffset.UtcNow);
    }
}


public class CreateReminderHandler(IServiceProvider serviceProvider) : BaseHandler<CreateReminderCommand, Guid>(serviceProvider) {
    public override async Task<Guid> Handle(CreateReminderCommand request, CancellationToken cancellationToken) {
        ReminderDto model = request.Model;

        Domain.Entities.Reminder entity = new() {
            Id        = Guid.NewGuid(),
            Title     = model.Title,
            Content   = model.Content ?? string.Empty,
            UserId    = model.UserId,
            Email     = model.Email,
            Frequency = model.Frequency,
            IsSent    = model.IsSent,
            SendDate  = model.SendDate,
            SendTime  = model.SendTime,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        await db.Reminders.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
