using T.Domain.Attributes;
using T.Domain.Models;

namespace T.Domain.Enums.Systems;

public enum EAction {
    [Action(EModule.Dashboard, "Xem", PermissionKey.Dashboard_View)]
    Dashboard_View = 10001,

    [Action(EModule.MerchantInformation, "Xem", PermissionKey.Merchant_View)]
    Merchant_View = 140201,
    [Action(EModule.MerchantInformation, "Sửa", PermissionKey.Merchant_Edit)]
    Merchant_Edit = 140202,

    [Action(EModule.User, "Xem", PermissionKey.User_View)]
    User_View = 140301,
    [Action(EModule.User, "Thêm/Sửa", PermissionKey.User_Edit)]
    User_Edit = 140302,
    [Action(EModule.User, "Xoá", PermissionKey.User_Delete)]
    User_Delete = 140303,
    [Action(EModule.User, "Đổi mật khẩu", PermissionKey.User_ChangePassword)]
    User_ChangePassword = 140306,
    [Action(EModule.User, "Đổi mã pin", PermissionKey.User_ChangePin)]
    User_ChangePin = 140307,

    [Action(EModule.Role, "Xem", PermissionKey.Role_View)]
    Role_View = 140401,
    [Action(EModule.Role, "Thêm/Sửa", PermissionKey.Role_Edit)]
    Role_Edit = 140402,
    [Action(EModule.Role, "Xoá", PermissionKey.Role_Delete)]
    Role_Delete = 140403,
}
