using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace Rentityx.Users.AppHost;

internal static class ResourceBuilderExtensions
{
    private static IResourceBuilder<T> WithOpenApiDocs<T>(
        this IResourceBuilder<T> builder,
        string name,
        string displayName,
        string openApiUiPath)
        where T : IResourceWithEndpoints
    {
        return builder.WithCommand(
            name,
            displayName,
            executeCommand: _ => 
            {
                try
                {
                    var endpoint = builder.GetEndpoint("https");

                    var url = $"{endpoint.Url}/{openApiUiPath}";

                    Process.Start((new ProcessStartInfo(url) { UseShellExecute = true }));

                    return Task.FromResult(new ExecuteCommandResult { Success = true });
                }
                catch (InvalidOperationException ex)
                {
                    return Task.FromResult(new ExecuteCommandResult
                    {
                        Success = false,
                        ErrorMessage = "Invalid operation: " + ex.Message
                    });
                }
            },
            updateState: context =>
                context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy
                    ? ResourceCommandState.Enabled
                    : ResourceCommandState.Disabled,
            iconName: "Document",
            iconVariant: IconVariant.Filled);
    }

    internal static IResourceBuilder<T> WithSwaggerUi<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs(
            name: "swagger - ui - docs",
            displayName: "Swagger API Documentation",
            openApiUiPath: "swagger");
    }

    internal static IResourceBuilder<T> WithScalar<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs(
            name: "scalar-docs",
            displayName: "Scalar API Documentation",
            openApiUiPath: "scalar/v1");
    }

    internal static IResourceBuilder<T> WithReDoc<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs(
            name: "redoc-docs",
            displayName: "Redoc API Documentation",
            openApiUiPath: "api-docs");
    }
}

