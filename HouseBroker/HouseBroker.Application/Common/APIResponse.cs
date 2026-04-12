using System.Net;

namespace HouseBroker.Application.Common;

public class APIResponse
{
    public APIResponse(object? data, List<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        StatusCode = statusCode;
        Data = data;
        Errors = errors ?? [];
    }
    public HttpStatusCode StatusCode { get; set; }
    public object? Data { get; set; } = null;
    public List<string>? Errors { get; set; } = null;
}