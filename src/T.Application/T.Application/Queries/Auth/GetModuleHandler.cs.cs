#region

using T.Application.Base;
using T.Application.Models.SystemDto;
using T.Domain.Enums.Systems;

#endregion

namespace T.Application.Queries.Auth;

public class GetModuleQuery : IRequest<ModuleData> { }


public class ModuleData
{
    public List<ModuleDto> Modules { get; set; } = [];
    public List<ActionDto> Actions { get; set; } = [];
}


public class GetModuleHandler(IServiceProvider serviceProvider) : BaseHandler(serviceProvider),
    IRequestHandler<GetModuleQuery, ModuleData>
{
    public async Task<ModuleData> Handle(GetModuleQuery request, CancellationToken cancellationToken)
    {
        ModuleData data = new();

        Dictionary<EAction, ActionRelation> relations = GetActionRelations().ToDictionary(o => o.For);
        data.Actions = Enum.GetValues<EAction>()
            .Select(
                a =>
                {
                    ActionRelation? rel = relations.GetValueOrDefault(a);
                    return ActionDto.From(
                        a,
                        rel?.Check ?? [],
                        rel?.Uncheck ?? [],
                        rel?.Description);
                })
            .OrderBy(o => o.Action)
            .ToList();

        var modules = Enum.GetValues<EModule>()
            .Select(
                m => new
                {
                    Div    = (int)m / 10000,
                    Mod    = (int)m % 10000,
                    Module = m,
                })
            .ToList();

        var parents = modules.Where(m => m.Mod == 0).OrderBy(o => o.Module).ToList();
        foreach (var parent in parents)
        {
            var item = ModuleDto.From(parent.Module);
            item.Children = modules.Where(m => m.Div == parent.Div && m.Mod != 0)
                .OrderBy(o => o.Mod)
                .Select(
                    m =>
                    {
                        var i = ModuleDto.From(m.Module);
                        i.NumberOfActions = data.Actions.FindAll(a => a.Module == i.Module).Count;
                        return i;
                    })
                .ToList();

            item.NumberOfActions = item.Children.Count > 0 ? item.Children.Count : data.Actions.FindAll(a => a.Module == item.Module).Count;

            data.Modules.Add(item);
        }

        return await Task.FromResult(data);
    }

    private IEnumerable<ActionRelation> GetActionRelations()
    {
        // merchant
        yield return new ActionRelation(EAction.MerchantView).WithUncheck(EAction.MerchantEdit);
        yield return new ActionRelation(EAction.MerchantEdit).WithCheck(EAction.MerchantView);

        // user
        yield return new ActionRelation(EAction.UserView).WithUncheck(
            EAction.UserEdit,
            EAction.UserDelete,
            EAction.UserChangePassword,
            EAction.UserChangePin);
        yield return new ActionRelation(EAction.UserEdit).WithCheck(EAction.UserView);
        yield return new ActionRelation(EAction.UserDelete).WithCheck(EAction.UserView);
        yield return new ActionRelation(EAction.UserChangePassword).WithCheck(EAction.UserView);
        yield return new ActionRelation(EAction.UserChangePin).WithCheck(EAction.UserView);

        // role
        yield return new ActionRelation(EAction.RoleView).WithUncheck(EAction.RoleEdit, EAction.RoleDelete);
        yield return new ActionRelation(EAction.RoleEdit).WithCheck(EAction.RoleView);
        yield return new ActionRelation(EAction.RoleDelete).WithCheck(EAction.RoleView);
    }


    private class ActionRelation(EAction action)
    {
        public EAction       For         { get; }      = action;
        public string        Description { get; set; } = string.Empty;
        public List<EAction> Check       { get; set; } = [];
        public List<EAction> Uncheck     { get; set; } = [];

        internal ActionRelation WithCheck(params EAction[] actions)
        {
            Check = actions.ToList();
            return this;
        }

        internal ActionRelation WithUncheck(params EAction[] actions)
        {
            Uncheck = actions.ToList();
            return this;
        }

        internal ActionRelation WithDescription(string description)
        {
            Description = description;
            return this;
        }
    }
}
