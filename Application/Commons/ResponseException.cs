using System;

namespace Application.Commons
{
    public class ResponseException : Exception
    {
        public int Status { get; }
        public override string Message { get; }
        public object Detail { get; }

        public ResponseException(int status, string message, object detail = null)
        {
            Status = status;
            Message = message;
            Detail = detail;
        }
    }
}