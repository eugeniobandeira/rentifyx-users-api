using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Rentifyx.Users.Application.Features.Users.Handler.Create;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Validator;
using Rentifyx.Users.Application.Features.Users.Handler.GetByDocument;
using System.Diagnostics.CodeAnalysis;

namespace Rentifyx.Users.IoC;

[ExcludeFromCodeCoverage]
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
        services.AddScoped<IGetUserByDocumentHandler, GetUserByDocumentHandler>();
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUserRequestDto>, CreateUserValidator>();
    }
}
