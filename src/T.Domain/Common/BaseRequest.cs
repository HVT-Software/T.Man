#region

using MediatR;

#endregion

namespace T.Domain.Common;

public abstract class BaseRequest {
    public Guid UserId     { get; set; }
    public Guid MerchantId { get; set; }
}


public abstract class Request
    : BaseRequest,
        IRequest { }


public abstract class Request<TResponse>
    : BaseRequest,
        IRequest<TResponse> { }


public abstract class BaseSingleRequest : BaseRequest {
    public Guid Id { get; set; }
}


public class SingleRequest
    : BaseSingleRequest,
        IRequest { }


public class SingleRequest<TResponse>
    : BaseSingleRequest,
        IRequest<TResponse> { }


public abstract class BasePaginatedRequest : BaseRequest {
    public DateTimeOffset? From      { get; set; }
    public DateTimeOffset? To        { get; set; }
    public int             PageIndex { get; set; }
    public int             PageSize  { get; set; }

    public bool    IsAll      { get; set; }
    public bool    IsCount    { get; set; }
    public string? SearchText { get; set; }

    public int Take { get => PageSize; }
    public int Skip { get => Math.Max(PageIndex - 1, 0) * PageSize; }
}


public class PaginatedRequest
    : BasePaginatedRequest,
        IRequest { }


public class PaginatedRequest<TResponse>
    : BasePaginatedRequest,
        IRequest<TResponse> { }


public abstract class BaseUpdateRequest<T> : BaseSingleRequest where T : notnull {
    public required T Model { get; set; }
}


public class UpdateRequest<T>
    : BaseUpdateRequest<T>,
        IRequest where T : notnull { }


public class UpdateRequest<T, TResponse>
    : BaseUpdateRequest<T>,
        IRequest<TResponse> where T : notnull { }


public record WrapperData<T> {
    public int      TotalCount { get; set; }
    public List<T>? Items      { get; set; }
}
