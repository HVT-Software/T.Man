namespace T.Domain.Constants;

public static class RedisKey
{
    public static string GetGlobalSettingKey(string env, Guid merchantId)
    {
        return$"{env}:{merchantId}:GlobalSetting";
    }

    public static string GetSessionKey(
        string env,
        Guid merchantId,
        Guid userId)
    {
        return$"{env}:{merchantId}:Session:{userId}";
    }

    public static string GetSessionActionKey(
        string env,
        Guid merchantId,
        Guid userId)
    {
        return$"{env}:{merchantId}:Session:{userId}:Action";
    }

    public static string GetHeaderSystemKey(string merchantCode)
    {
        return$"MerchantCode:{merchantCode}";
    }
}
