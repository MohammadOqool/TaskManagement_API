using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManagementSystemAPI.Models.Swagger
{
    public class SwaggerGlobalErrorResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var problemResponseSchema = context.SchemaGenerator.GenerateSchema(typeof(ApiResponse<>), context.SchemaRepository);

            // Add 400 BadRequest response
            operation.Responses.TryAdd("400", new OpenApiResponse
            {
                Description = "Bad Request",
                Content =
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = problemResponseSchema,
                    Example = new Microsoft.OpenApi.Any.OpenApiObject
                    {
                        ["success"] = new Microsoft.OpenApi.Any.OpenApiBoolean(false),
                        ["errorMessage"] = new Microsoft.OpenApi.Any.OpenApiString("Missing inputs"),
                        ["data"] = new Microsoft.OpenApi.Any.OpenApiNull()
                    }
                }
            }
            });

            // Add 500 InternalServerError response
            operation.Responses.TryAdd("500", new OpenApiResponse
            {
                Description = "Internal Server Error",
                Content =
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = problemResponseSchema,
                    Example = new Microsoft.OpenApi.Any.OpenApiObject
                    {
                        ["success"] = new Microsoft.OpenApi.Any.OpenApiBoolean(false),
                        ["errorMessage"] = new Microsoft.OpenApi.Any.OpenApiString("Internal server error"),
                        ["data"] = new Microsoft.OpenApi.Any.OpenApiNull()
                    }
                }
            }
            });
        }
    }
}
