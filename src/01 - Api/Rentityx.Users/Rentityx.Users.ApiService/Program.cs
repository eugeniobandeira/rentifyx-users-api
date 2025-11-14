using Rentifyx.Users.ApiService.Extensions;
using Rentifyx.Users.ServiceDefaults;
using Scalar.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
        options.RoutePrefix = "swagger";
    });

    app.UseReDoc(options =>
    {
        options.SpecUrl("/openapi/v1.json");
        options.RoutePrefix = "api-docs";
    });

    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

app.MapEndpoints();

await app.RunAsync();


/// <summary>
/// Provides the entry point for the application.
/// </summary>
/// <remarks>It is necessary to create integration tests</remarks>
[ExcludeFromCodeCoverage]
public partial class Program { }    
