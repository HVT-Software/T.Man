using System.ComponentModel;

namespace T.Domain.Enums.Systems;
public enum EModule {
    [Description("Tổng quan")]
    Dashboard = 10000,

    [Description("Quản lý sản phẩm")]
    ProductManagement = 20000,
    [Description("Danh sách sản phẩm")]
    Product = 20100,
    [Description("Danh mục sản phẩm")]
    Category = 20200,
    [Description("Thương hiệu")]
    Brand = 20300,

    [Description("Quản lý khách hàng")]
    CustomerManagement = 30000,
    [Description("Danh sách khách hàng")]
    Customer = 30100,
    [Description("Nhóm khách hàng")]
    CustomerGroup = 30200,
    [Description("Phiếu nợ")]
    DebitTicket = 30300,

    [Description("Quản lý nhân sự")]
    EmployeeManagement = 40000,
    [Description("Danh sách nhân viên")]
    Employee = 40100,

    [Description("Quản lý đơn hàng")]
    OrderManagement = 50000,
    [Description("Danh sách đơn hàng")]
    Order = 50100,
    [Description("Danh sách đơn mua hàng")]
    PurchaseOrder = 50200,

    [Description("Quản lý vận chuyển")]
    TransportManagement = 60000,
    [Description("Phương tiện vận chuyển")]
    Vehicle = 60100,
    [Description("Phiếu giao hàng")]
    DeliveryTicket = 60200,

    [Description("Quản lý công nợ")]
    DebtManagement = 70000,
    [Description("Danh sách thu/chi")]
    ReceiptPayment = 70100,
    [Description("Công nợ khách hàng")]
    CustomerDebt = 70200,

    [Description("Quản lý nhà cung cấp")]
    SupplierManagement = 80000,
    [Description("Danh sách nhà cung cấp")]
    Supplier = 80100,

    [Description("Quản lý kho")]
    WarehouseManagement = 90000,
    [Description("Danh sách kho")]
    Wh = 90100,
    [Description("Tồn kho")]
    Stock = 90200,
    [Description("Phiếu nhập kho")]
    WhImp = 90300,
    [Description("Phiếu xuất kho")]
    WhExp = 90400,
    [Description("Phiếu chuyển kho")]
    WhTrf = 90500,
    [Description("Phiếu trả hàng")]
    WhRfd = 90600,
    [Description("Phiếu kiểm kho")]
    WhAdj = 90700,

    [Description("Quản lý cửa hàng")]
    StoreManagement = 100000,
    [Description("Danh sách cửa hàng")]
    Store = 100100,

    [Description("Quản lý thiết bị POS")]
    DeviceManagement = 110000,
    [Description("Danh sách thiết bị POS")]
    Device = 110100,

    [Description("Báo cáo")]
    Report = 120000,
    [Description("Báo cáo bán hàng")]
    ReportSale = 120100,
    [Description("Báo cáo danh mục")]
    ReportCategory = 120200,
    [Description("Báo cáo khách hàng")]
    ReportCustomer = 120300,
    [Description("Báo cáo sản phẩm")]
    ReportProduct = 120400,
    [Description("Báo cáo nhập hàng")]
    ReportWhImp = 120500,
    [Description("Báo cáo xuất hàng")]
    ReportWhExp = 120600,
    [Description("Báo cáo công nợ")]
    ReportCustomerDebt = 120700,
    [Description("Báo cáo mua hàng")]
    ReportPurchaseOrder = 120800,

    [Description("Lịch sử thao tác")]
    Audit = 130000,

    [Description("Cài đặt")]
    Setting = 140000,
    [Description("Cài đặt chung")]
    General = 140100,
    [Description("Thông tin chung")]
    MerchantInformation = 140200,
    [Description("Người dùng")]
    User = 140300,
    [Description("Phân quyền")]
    Role = 140400,
    [Description("Phương thức thanh toán")]
    PaymentMethod = 140500,
    [Description("Yêu cầu")]
    Request = 140600,
}
