using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskTracker.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Response(T data, HttpStatusCode statusCode, string? message = null)
        {
            var defaultMessage = statusCode switch
            {
                HttpStatusCode.OK => "Success",
                HttpStatusCode.NotFound => "Not Found",
                HttpStatusCode.BadRequest => "Bad Request",
                HttpStatusCode.InternalServerError => "Internal Server Error",
                _ => statusCode.ToString()
            };

            return new ApiResponse<T>
            {
                Data = data,
                StatusCode = (int)statusCode,
                Message = string.IsNullOrEmpty(message) ? defaultMessage : message
            };
        }

        public static ApiResponse<Dictionary<string, string[]>> ValidationError(ModelStateDictionary modelState, string? message)
        {
            var errors = modelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return new ApiResponse<Dictionary<string, string[]>>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = string.IsNullOrEmpty(message) ? "Validation Error" : message,
                Data = errors
            };
        }

        public static ApiResponse<T> Success(T data, string? message = null) =>
            Response(data, HttpStatusCode.OK, message);
        public static ApiResponse<T> Created(T data, string? message = null) =>
            Response(data, HttpStatusCode.Created, message);
        public static ApiResponse<T> NotFound(string? message = null) =>
            Response(default!, HttpStatusCode.NotFound, message);
        public static ApiResponse<Dictionary<string, string[]>> BadRequest(ModelStateDictionary modelState, string? message = null) =>
            ValidationError(modelState, message);
        public static ApiResponse<T> BadRequest(string message) => 
            Response(default!, HttpStatusCode.BadRequest, message);

        public static ApiResponse<T> InternalServerError(T data, string? message = null) =>
            Response(data, HttpStatusCode.InternalServerError, message);
    }
}
