namespace T.Domain.Models;

public static class PermissionKey {
    private const string DefaultView    = "r";
    private const string DefaultEdit    = "cu";
    private const string DefaultDelete  = "d";
    private const string DefaultImport  = "im";
    private const string DefaultExport  = "ex";
    private const string DefaultHistory = "hx";

#region 1. Dashboard

    private const string DashboardPage   = "db";
    private const string DashboardModule = "db";

    public const string DashboardView = $"{DashboardPage}_{DashboardModule}_{DefaultView}";

#endregion

#region 2. Setting

    private const string SettingPage = "stg";

    // 2.1. Merchant
    private const string MerchantModule = "mer";

    public const string MerchantView = $"{SettingPage}_{MerchantModule}_{DefaultView}";
    public const string MerchantEdit = $"{SettingPage}_{MerchantModule}_{DefaultEdit}";

    // 2.2. User
    private const string UserModule = "usr";

    public const string UserView           = $"{SettingPage}_{UserModule}_{DefaultView}";
    public const string UserEdit           = $"{SettingPage}_{UserModule}_{DefaultEdit}";
    public const string UserDelete         = $"{SettingPage}_{UserModule}_{DefaultDelete}";
    public const string UserChangePassword = $"{SettingPage}_{UserModule}_pw";
    public const string UserChangePin      = $"{SettingPage}_{UserModule}_pin";

    // 2.3. Role
    private const string RoleModule = "role";

    public const string RoleView   = $"{SettingPage}_{RoleModule}_{DefaultView}";
    public const string RoleEdit   = $"{SettingPage}_{RoleModule}_{DefaultEdit}";
    public const string RoleDelete = $"{SettingPage}_{RoleModule}_{DefaultDelete}";

#endregion

#region 3. Money tracking

    private const string MoneyTrackingPage = "stg";

    // 3.1. Category
    private const string CategoryModule = "cat";

    public const string CategoryView   = $"{MoneyTrackingPage}_{CategoryModule}_{DefaultView}";
    public const string CategoryEdit   = $"{MoneyTrackingPage}_{CategoryModule}_{DefaultEdit}";
    public const string CategoryDelete = $"{MoneyTrackingPage}_{CategoryModule}_{DefaultDelete}";

    // 3.2. Transaction

    private const string TransactionModule = "tra";

    public const string TransactionView   = $"{MoneyTrackingPage}_{TransactionModule}_{DefaultView}";
    public const string TransactionEdit   = $"{MoneyTrackingPage}_{TransactionModule}_{DefaultEdit}";
    public const string TransactionDelete = $"{MoneyTrackingPage}_{TransactionModule}_{DefaultDelete}";

#endregion
}
