#region

using System.ComponentModel;

#endregion

namespace T.Domain.Enums.Systems;

public enum EModule {
    [Description("Tổng quan")]
    Dashboard = 10000,

    [Description("Cài đặt")]
    Setting = 110000,

    [Description("Thông tin chung")]
    MerchantInformation = 110100,

    [Description("Người dùng")]
    User = 110200,

    [Description("Phân quyền")]
    Role = 110300,

    [Description("Quản lý tài chính")]
    MoneyTracking = 120000,

    [Description("Danh mục")]
    Category = 120100,

    [Description("Giao dịch")]
    Transaction = 120200,

    [Description("Công nợ")]
    Debt = 120300,
}
