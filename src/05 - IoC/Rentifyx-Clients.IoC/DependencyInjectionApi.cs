using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RentifyxClients.IoC;
public static class DependencyInjectionApi
{
    //public static IHostApplicationBuilder UseOpenApiTemplate(this IHostApplicationBuilder builder)
    //{
    //    builder.Services.AddOpenApi(options =>
    //    {
    //        options.AddDocumentTransformer((document, context, cancellationToken) =>
    //        {
    //            document.Info.Title = "Rentifyx API";
                
    //            return Task.CompletedTask;
    //        });
    //    });

    //    return builder;
    //}

    public static WebApplication UseSwaggerUiTemplate(this WebApplication app)
    {
        app.MapOpenApi();
        
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Rentifyx");
        });

        return app;
    }

}
