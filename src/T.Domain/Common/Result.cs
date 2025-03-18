#region

using Newtonsoft.Json;
using T.Domain.Constants;

#endregion

namespace T.Domain.Common;

public class Result
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public object? Data { get; set; }

    public IDictionary<string, string[]>? Errors { get; set; }

    public static Result Ok()
    {
        return new Result
        {
            Success = true,
        };
    }

    public static Result Ok<T>(T? data)
    {
        return new Result
        {
            Success = true,
            Data    = data,
        };
    }

    public static Result Fail(string? message = null)
    {
        return new Result
        {
            Message = message,
        };
    }

    public static Result Fail(IDictionary<string, string[]>? errors = null)
    {
        return new Result
        {
            Message = Messages.Validation_Fail,
            Errors  = errors,
        };
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}


public class FileResult
{
    public string FileName  { get; set; } = string.Empty;
    public byte[] ByteArray { get; set; } = [];
}
