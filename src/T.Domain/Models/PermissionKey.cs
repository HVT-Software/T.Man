namespace T.Domain.Models;

public static class PermissionKey {
    private const string DefaultView = "r";
    private const string DefaultEdit = "cu";
    private const string DefaultDelete = "d";
    private const string DefaultImport = "im";
    private const string DefaultExport = "ex";
    private const string DefaultHistory = "hx";

    // 1. Dashboard
    private const string DashboardPage = "db";
    private const string DashboardModule = "db";

    public const string Dashboard_View = $"{DashboardPage}_{DashboardModule}_{DefaultView}";

    // 14. Setting
    private const string SettingPage = "stg";

    // 14.1. Merchant
    private const string MerchantModule = "mer";

    public const string Merchant_View = $"{SettingPage}_{MerchantModule}_{DefaultView}";
    public const string Merchant_Edit = $"{SettingPage}_{MerchantModule}_{DefaultEdit}";

    // 14.2. User
    private const string UserModule = "usr";

    public const string User_View = $"{SettingPage}_{UserModule}_{DefaultView}";
    public const string User_Edit = $"{SettingPage}_{UserModule}_{DefaultEdit}";
    public const string User_Delete = $"{SettingPage}_{UserModule}_{DefaultDelete}";
    public const string User_ChangePassword = $"{SettingPage}_{UserModule}_pw";
    public const string User_ChangePin = $"{SettingPage}_{UserModule}_pin";

    // 14.3. Role
    private const string RoleModule = "role";

    public const string Role_View = $"{SettingPage}_{RoleModule}_{DefaultView}";
    public const string Role_Edit = $"{SettingPage}_{RoleModule}_{DefaultEdit}";
    public const string Role_Delete = $"{SettingPage}_{RoleModule}_{DefaultDelete}";
}
