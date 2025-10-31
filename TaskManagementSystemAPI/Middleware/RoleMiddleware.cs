namespace TaskManagementSystemAPI.Middleware
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            // skip middleware for Swagger, OpenAPI, and root endpoints
            if (!path!.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // Simulate authentication from headers
            //var role = context.Request.Headers["X-Role"].ToString();
            var userId = context.Request.Headers["X-UserId"].ToString();

            // If either header is missing, return an error
            if (/*string.IsNullOrEmpty(role) || */string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    //error = "Missing authentication headers. Please provide both X-Role and X-UserId."
                    error = "Missing authentication headers. Please provide X-UserId."
                });
                return;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                context.Items["UserRole"] = userId;
                if (int.TryParse(userId, out var id))
                    context.Items["UserId"] = id;
            }

            await _next(context);
        }
    }

    public static class RoleMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<RoleMiddleware>();
    }
}
