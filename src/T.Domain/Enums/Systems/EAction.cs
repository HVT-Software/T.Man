#region

using T.Domain.Attributes;
using T.Domain.Models;

#endregion

namespace T.Domain.Enums.Systems;

public enum EAction {
    [Action(EModule.Dashboard, "Xem", PermissionKey.DashboardView)]
    DashboardView = 10001,

#region Setting

    // Merchant
    [Action(EModule.MerchantInformation, "Xem", PermissionKey.MerchantView)]
    MerchantView = 110101,

    [Action(EModule.MerchantInformation, "Sửa", PermissionKey.MerchantEdit)]
    MerchantEdit = 110102,

    // User
    [Action(EModule.User, "Xem", PermissionKey.UserView)]
    UserView = 110201,

    [Action(EModule.User, "Thêm/Sửa", PermissionKey.UserEdit)]
    UserEdit = 110202,

    [Action(EModule.User, "Xoá", PermissionKey.UserDelete)]
    UserDelete = 110203,

    [Action(EModule.User, "Đổi mật khẩu", PermissionKey.UserChangePassword)]
    UserChangePassword = 110206,

    [Action(EModule.User, "Đổi mã pin", PermissionKey.UserChangePin)]
    UserChangePin = 110207,

    // Role
    [Action(EModule.Role, "Xem", PermissionKey.RoleView)]
    RoleView = 110301,

    [Action(EModule.Role, "Thêm/Sửa", PermissionKey.RoleEdit)]
    RoleEdit = 110302,

    [Action(EModule.Role, "Xoá", PermissionKey.RoleDelete)]
    RoleDelete = 110303,

#endregion

#region Monney Tracking

    [Action(EModule.Category, "Xem", PermissionKey.CategoryView)]
    CategoryView = 120101,

    [Action(EModule.Category, "Thêm/Sửa", PermissionKey.CategoryEdit)]
    CategoryEdit = 120102,

    [Action(EModule.Category, "Xoá", PermissionKey.CategoryDelete)]
    CategoryDelete = 120103,

#endregion
}
