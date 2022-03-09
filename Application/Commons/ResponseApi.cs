using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commons
{
    public class ResponseApi<TData> : IRequest<Unit>
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }

        public static ResponseApi<TData> ResponseOk(TData data) => new()
        {
            Success = true,
            Status = StatusCodes.Status200OK,
            Data = data
        };

        public static ResponseApi<TData> ResponseOk(TData data, string message) => new()
        {
            Success = true,
            Status = StatusCodes.Status200OK,
            Data = data,
            Message = message
        };

        public static ResponseApi<TData> ResponseOk(TData data, int status, string message) => new()
        {
            Success = true,
            Status = status,
            Data = data,
            Message = message
        };
        
        public static ResponseApi<TData> ResponseFail(string message) => new()
        {
            Success = false,
            Status = StatusCodes.Status400BadRequest,
            Message = message
        };
        
        public static ResponseApi<TData> ResponseFail(int status, string message) => new()
        {
            Success = false,
            Status = status,
            Message = message
        };
    }
}