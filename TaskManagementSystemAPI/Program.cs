using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TaskManagementSystemAPI.Interfaces;
using TaskManagementSystemAPI.Managers.Loggers;
using TaskManagementSystemAPI.Middleware;
using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.Context;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Swagger;
using TaskManagementSystemAPI.ServicesLayers.Implementations;
using TaskManagementSystemAPI.ServicesLayers.Interfaces;
using TaskManagementSystemAPI.UnitOfWorkPattern;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson();

// configure in memory db
builder.Services.AddDbContext<AppDBContext>(opption =>
        opption.UseLazyLoadingProxies()
                .UseInMemoryDatabase("in_memory")
    );

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// inject all the generic services layers
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();
// Inject the LoggerManager
builder.Services.AddSingleton<ILoggerManager, FileLoggerManager>();
builder.Services.AddSingleton<LoggerHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var xRoleHeader = new OpenApiParameter
        {
            Name = "X-Role",
            In = ParameterLocation.Header,
            Required = false,
            Description = "User role header (Admin/User)",
            Schema = new OpenApiSchema { Type = "string" }
        };

        var xUserIdHeader = new OpenApiParameter
        {
            Name = "X-UserId",
            In = ParameterLocation.Header,
            Required = false,
            Description = "User ID header (integer)",
            Schema = new OpenApiSchema { Type = "integer", Format = "int32" }
        };

        foreach (var path in document.Paths.Values)
        {
            foreach (var operation in path.Operations.Values)
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                //operation.Parameters.Add(xRoleHeader);
                operation.Parameters.Add(xUserIdHeader);
            }
        }

        return Task.CompletedTask;
    });
});

// Enable Swagger
/*builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("X-UserId", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-UserId",
        In = ParameterLocation.Header,
        Description = "User ID"
    });

    *//*c.AddSecurityDefinition("X-Role", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Role",
        In = ParameterLocation.Header,
        Description = "User Role"
    });*//*

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
   *//* {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "X-UserId" }
        },
        new List<string>()
    },*//*
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "X-Role" }
        },
        new List<string>()
    }
});


    c.OperationFilter<SwaggerGlobalErrorResponsesOperationFilter>();
    //c.ExampleFilters();
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

    /*app.UseSwagger();
    app.UseSwaggerUI();*/

}

// to use swagger page as default page
var rewriteOptions = new RewriteOptions();
rewriteOptions.AddRedirect("^$", "swagger");
app.UseRewriter(rewriteOptions);

app.UseRoleMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
