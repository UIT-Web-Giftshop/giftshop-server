namespace Application.Commons
{
    public class ResponseApi<T>
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public static ResponseApi<T> ResponseOk(T data) => new()
        {
            IsSuccess = true,
            Data = data
        };

        public static ResponseApi<T> ResponseOk(T data, string message) => new()
        {
            IsSuccess = true,
            Code = 200,
            Data = data,
            Message = message
        };

        public static ResponseApi<T> ResponseOk(T data, int code, string message) => new()
        {
            IsSuccess = true,
            Code = code,
            Data = data,
            Message = message
        };
        
        public static ResponseApi<T> ResponseFail(string message) => new()
        {
            IsSuccess = false,
            Code = 500,
            Message = message
        };
        
        public static ResponseApi<T> ResponseFail(int code, string message) => new()
        {
            IsSuccess = false,
            Code = code,
            Message = message
        };
    }
}