using System.Net;

namespace TaskManagementSystemAPI.Models
{
    // class to implement resposne from service layer
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string? ErrorMessage { get; set; }

        // Static methods
        public static ServiceResult<T> Success(T? data, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            new ServiceResult<T> { Data = data, IsSuccess = true, StatusCode = statusCode };

        public static ServiceResult<T> Failure(string? errorMessage, HttpStatusCode statusCode = HttpStatusCode.NotFound) =>
            new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage, StatusCode = statusCode };
    }
}
