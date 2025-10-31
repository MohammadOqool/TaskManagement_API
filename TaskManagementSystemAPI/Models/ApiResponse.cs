using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagementSystemAPI.Models
{
    public class ApiResponse<T>
    {
        public bool success;
        public string? errorMessage;
        public T? data;

        public ApiResponse(bool success = true)
        {
            this.success = success;
        }

/*        public ApiResponse(int statusCode, string message, T? data = default)
        {
            this.success = statusCode;
            this.errorMessage = message;
            this.data = data;
        }

        public static ApiResponse<T> Success(object? data = null, string? message = null) => new ApiResponse<T>(200, message ?? "Success", data);

        public static ApiResponse<T> Fail(int statusCode, string message) => new ApiResponse<T>(statusCode, message);
*/    }
}
