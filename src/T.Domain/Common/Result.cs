#region

using Newtonsoft.Json;
using T.Domain.Constants;

#endregion

namespace T.Domain.Common;

public class Result {
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }

    public IDictionary<string, string[]>? Errors { get; set; }

    public static Result Ok() {
        return new Result {
            Success = true,
        };
    }

    public static Result Fail(string? message = null) {
        return new Result {
            Message = message,
        };
    }

    public static Result Fail(IDictionary<string, string[]>? errors = null) {
        return new Result {
            Message = Messages.Validation_Fail,
            Errors  = errors,
        };
    }

    public override string ToString() {
        return JsonConvert.SerializeObject(this);
    }
}


public class Result<T> : Result {
    public T? Data { get; set; }

    public static Result<T> Ok(T? data) {
        return new Result<T> {
            Success = true,
            Data    = data,
        };
    }

    public new static Result<T> Fail(string? message = null) {
        return new Result<T> {
            Message = message,
        };
    }

    public new static Result<T> Fail(IDictionary<string, string[]>? errors = null) {
        return new Result<T> {
            Message = Messages.Validation_Fail,
            Errors  = errors,
        };
    }
}


public class FileResult {
    public string FileName  { get; set; } = string.Empty;
    public byte[] ByteArray { get; set; } = [];
}
