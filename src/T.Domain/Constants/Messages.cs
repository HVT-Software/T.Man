namespace T.Domain.Constants;

public static class Messages
{
    public const string Success         = "Thành công";
    public const string Error           = "Lỗi hệ thống";
    public const string BadRequest      = "Yêu cầu không hợp lệ";
    public const string NotFound        = "Không tìm thấy dữ liệu";
    public const string Unauthorized    = "Không có quyền truy cập";
    public const string Validation_Fail = "Dữ liệu không hợp lệ";

    public const string User_NotFound          = "Người dùng không tồn tại";
    public const string User_Inactive          = "Người dùng đã bị vô hiệu hóa";
    public const string User_IncorrectPassword = "Mật khẩu không chính xác";
    public const string User_NoPermission      = "Người dùng không có quyền truy cập";
    public const string User_NameIsRequire     = "Tên người dùng không được để trống";
    public const string User_NotAllowAccess    = "Không có quyền để truy cập hệ thống";

    public const string Category_NotFound = "Nhóm không tồn tại";
    public const string Category_Used     = "Nhóm đang được sử dụng";

    public const string Merchant_NotFound = "Merchant không tồn tại";
}
