namespace H.Domain.Constants;

public class Constants {
    public const string TokenUserId = "UserId";
    public const string TokenRefreshToken = "RefreshToken";
    public const string TokenSource = "Source";
    public const string TokenSession = "Session";
}

public static class RedisKey {

    public static string GetSuggestBestSellingKey(string merchantId, string storeId, string? warehouseId) {
        return $"Merchant:{merchantId}:SuggestBestSelling:StoreId_{storeId}:WarehouseId_{warehouseId ?? "null"}";
    }

    public static string GetGlobalSettingKey(string merchantId) {
        return $"Merchant:{merchantId}:GlobalSetting";
    }

    public static string GetSessionKey(string source, string userId) {
        return $"Session:{source}:{userId}";
    }

    public static string GetHeaderSystemKey(string merchantCode) {
        return $"MerchantCode:{merchantCode}";
    }
}
