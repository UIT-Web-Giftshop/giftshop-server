using Microsoft.AspNetCore.Http;

namespace Application.Commons
{
    public class ResponseApi<TData>
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }

        public static ResponseApi<TData> ResponseOk(TData data) => new()
        {
            IsSuccess = true,
            Code = StatusCodes.Status200OK,
            Data = data
        };

        public static ResponseApi<TData> ResponseOk(TData data, string message) => new()
        {
            IsSuccess = true,
            Code = StatusCodes.Status200OK,
            Data = data,
            Message = message
        };

        public static ResponseApi<TData> ResponseOk(TData data, int code, string message) => new()
        {
            IsSuccess = true,
            Code = code,
            Data = data,
            Message = message
        };
        
        public static ResponseApi<TData> ResponseFail(string message) => new()
        {
            IsSuccess = false,
            Code = StatusCodes.Status400BadRequest,
            Message = message
        };
        
        public static ResponseApi<TData> ResponseFail(int code, string message) => new()
        {
            IsSuccess = false,
            Code = code,
            Message = message
        };
    }
}