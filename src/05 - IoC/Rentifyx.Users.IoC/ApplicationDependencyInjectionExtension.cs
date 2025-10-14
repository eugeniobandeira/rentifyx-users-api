using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Rentifyx.Users.Application.Features.Users.Handler.Create;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Validator;

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
        services.AddScoped<ICreateUserHandler, CreateUserHandler>();
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUserRequestDto>, CreateUserValidator>();
    }
}
