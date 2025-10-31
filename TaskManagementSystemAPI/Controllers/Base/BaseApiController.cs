using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using TaskManagementSystemAPI.Managers.Loggers;
using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.ServicesLayers.Interfaces;


namespace TaskManagementSystemAPI.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected LoggerHandler loggerHandler;
        protected HttpContext httpContext;
        protected IUserService userService;

        protected string? currentRole;
        protected int? currentUserId;

        public BaseApiController(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
            loggerHandler = httpContextAccessor.HttpContext!.RequestServices.GetService<LoggerHandler>()!;
            userService = httpContextAccessor.HttpContext!.RequestServices.GetService<IUserService>()!;

            //currentRole = httpContext.Items["UserRole"]?.ToString();
            currentUserId = (int?)httpContext.Items["UserId"];
        }

        private async Task FetchUserRole()
        {
            var existingUser = await userService.Get((int)currentUserId);
            if (existingUser.IsSuccess)
            {
                currentRole = existingUser.Data?.Role;
            }
        }

        protected async Task<bool> IsAdminRole()
        {
            await FetchUserRole();
            return string.Equals(currentRole, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        protected ActionResult UnAuthorizedAccessResponse()
        {
            var apiResponse = new ApiResponse<object>()
            {
                errorMessage = "Unauthorized user to perform this action",
                data = null,
                success = false
            };
            return CreateApiResponse(apiResponse, HttpStatusCode.Unauthorized);
        }

        protected ActionResult<ApiResponse<T>> HandleResponse<T>(ServiceResult<T> result)
        {
            var apiResponse = new ApiResponse<T>()
            {
                errorMessage = result.ErrorMessage,
                data = result.Data,
                success = result.IsSuccess
            };
            return CreateApiResponse(apiResponse, result.StatusCode);
        }

        protected ActionResult HandleException(Exception exception)
        {
            loggerHandler?.Error(exception);
            var apiResponse = new ApiResponse<object>()
            {
                data = null,
                errorMessage = "Failed to execute operation",
                success = false
            };
            return CreateApiResponse(apiResponse, HttpStatusCode.InternalServerError);
        }

        protected ActionResult CreateApiResponse(object generalResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            AfterFinished();
            return StatusCode((int)statusCode, generalResponse);
            /*
            if (statusCode == HttpStatusCode.BadRequest)
                return BadRequest(generalResponse);

            if (statusCode == HttpStatusCode.NotFound)
                return NotFound(generalResponse);

            if (statusCode == HttpStatusCode.InternalServerError)
                return StatusCode(((int)statusCode), generalResponse);

            return Ok(generalResponse);*/
        }

        protected void AddCallingServiceMessage(string? endPointName = "")
        {
            if (string.IsNullOrEmpty(endPointName))
                endPointName = httpContext?.Request?.RouteValues["action"]?.ToString();

            string msg = $"Calling {endPointName} Service";
            loggerHandler.Info(msg);
        }
        protected void AfterFinished()
        {
            loggerHandler.Info("Finished execute Service");
            loggerHandler.AddLine();
        }
    }
}
