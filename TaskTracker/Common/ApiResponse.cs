using System.Net;

namespace TaskTracker.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Errors { get; set; }

        //public static ApiResponse<T> SuccessResponse(T data, string message, HttpStatusCode statusCode)
        //{

        //}

        //public static ApiResponse<T> FailureRe
    }
}
