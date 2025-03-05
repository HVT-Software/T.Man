using T.Application.Base;
using T.Application.Models.SystemDto;
using T.Domain.Enums.Systems;

namespace T.Application.Queries.Auth;


public class GetModuleQuery : IRequest<ModuleData> {
}

public class ModuleData {
    public List<ModuleDto> Modules { get; set; } = [];
    public List<ActionDto> Actions { get; set; } = [];
}

public class GetModuleHandler(IServiceProvider serviceProvider) : BaseHandler(serviceProvider), IRequestHandler<GetModuleQuery, ModuleData> {
    public async Task<ModuleData> Handle(GetModuleQuery request, CancellationToken cancellationToken) {
        var data = new ModuleData();

        var relations = this.GetActionRelations().ToDictionary(o => o.For);
        data.Actions = Enum.GetValues<EAction>().Select(a => {
            var rel = relations.GetValueOrDefault(a);
            return ActionDto.From(a, rel?.Check ?? [], rel?.Uncheck ?? [], rel?.Description);
        }).OrderBy(o => o.Action).ToList();

        var modules = Enum.GetValues<EModule>().Select(m => new {
            Div = (int)m / 10000,
            Mod = (int)m % 10000,
            Module = m,
        }).ToList();

        var parents = modules.Where(m => m.Mod == 0).OrderBy(o => o.Module).ToList();
        foreach (var parent in parents) {
            var item = ModuleDto.From(parent.Module);
            item.Children = modules.Where(m => m.Div == parent.Div && m.Mod != 0)
                .OrderBy(o => o.Mod)
                .Select(m => {
                    var i = ModuleDto.From(m.Module);
                    i.NumberOfActions = data.Actions.FindAll(a => a.Module == i.Module).Count;
                    return i;
                }).ToList();

            item.NumberOfActions = item.Children.Count > 0
                ? item.Children.Count
                : data.Actions.FindAll(a => a.Module == item.Module).Count;

            data.Modules.Add(item);
        }

        return await Task.FromResult(data);
    }

    private IEnumerable<ActionRelation> GetActionRelations() {

        // merchant
        yield return new ActionRelation(EAction.Merchant_View).WithUncheck(EAction.Merchant_Edit);
        yield return new ActionRelation(EAction.Merchant_Edit).WithCheck(EAction.Merchant_View);

        // user
        yield return new ActionRelation(EAction.User_View).WithUncheck(EAction.User_Edit, EAction.User_Delete, EAction.User_ChangePassword, EAction.User_ChangePin);
        yield return new ActionRelation(EAction.User_Edit).WithCheck(EAction.User_View);
        yield return new ActionRelation(EAction.User_Delete).WithCheck(EAction.User_View);
        yield return new ActionRelation(EAction.User_ChangePassword).WithCheck(EAction.User_View);
        yield return new ActionRelation(EAction.User_ChangePin).WithCheck(EAction.User_View);

        // role
        yield return new ActionRelation(EAction.Role_View).WithUncheck(EAction.Role_Edit, EAction.Role_Delete);
        yield return new ActionRelation(EAction.Role_Edit).WithCheck(EAction.Role_View);
        yield return new ActionRelation(EAction.Role_Delete).WithCheck(EAction.Role_View);
    }

    private class ActionRelation(EAction action) {
        public EAction For { get; } = action;
        public string Description { get; set; } = string.Empty;
        public List<EAction> Check { get; set; } = [];
        public List<EAction> Uncheck { get; set; } = [];

        internal ActionRelation WithCheck(params EAction[] actions) {
            this.Check = actions.ToList();
            return this;
        }

        internal ActionRelation WithUncheck(params EAction[] actions) {
            this.Uncheck = actions.ToList();
            return this;
        }

        internal ActionRelation WithDescription(string description) {
            this.Description = description;
            return this;
        }
    }
}
