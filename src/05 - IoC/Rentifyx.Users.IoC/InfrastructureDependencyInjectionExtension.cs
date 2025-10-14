using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;
using Rentifyx.Users.Infrastructure.Repositories;

namespace Rentifyx.Users.IoC;

public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IAddOnlyRepository<UserEntity>, UserRepository>();
    }   
}
