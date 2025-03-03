using H.Api.Common.Base;
using H.Api.Models;
using H.Domain.Constants;
using H.Domain.Extentions;
using Microsoft.EntityFrameworkCore;

namespace H.Api.Handlers.UserHandlers.Commands;

public class UpdateProfileCommand : ModelRequest<UserDto> {
}

internal class UpdateProfileHandler(IServiceProvider serviceProvider) : BaseHandler<UpdateProfileCommand>(serviceProvider) {

    public override async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken) {
        var model = request.Model;
        var user = await this.db.Users.FirstOrDefaultAsync(o => o.Id == model.Id && o.Username == model.Username && !o.IsDeleted && !o.IsSystem, cancellationToken);
        DrException.ThrowIfNull(user, Messages.User_NotFound);

        DrException.ThrowIf(user.IsAdmin && !model.IsActive, Messages.User_Inactive);
        DrException.ThrowIf(string.IsNullOrWhiteSpace(model.Name), Messages.User_NameIsRequire);

        //var originUser = user.Clone();
        user.Name = model.Name;
        user.Phone = model.Phone;
        user.Address = model.Address;

        await this.db.SaveChangesAsync(cancellationToken);
    }
}
