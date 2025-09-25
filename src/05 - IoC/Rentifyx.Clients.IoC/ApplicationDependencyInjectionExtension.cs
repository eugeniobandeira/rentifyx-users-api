using Microsoft.Extensions.DependencyInjection;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;

namespace Rentifyx.Clients.IoC;

public static class ApplicationDependencyInjectionExtension
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        AddHandlers(services);
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddScoped<ICreateClientHandler, CreateClientHandler>();
    }   
}
