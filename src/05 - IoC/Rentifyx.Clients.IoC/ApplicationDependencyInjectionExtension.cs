using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Validator;

namespace Rentifyx.Clients.IoC;

public static class ApplicationDependencyInjectionExtension
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        AddHandlers(services);
        AddValidators(services);
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddScoped<ICreateClientHandler, CreateClientHandler>();
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateClientRequestDto>, CreateClientValidator>();
    }
}
