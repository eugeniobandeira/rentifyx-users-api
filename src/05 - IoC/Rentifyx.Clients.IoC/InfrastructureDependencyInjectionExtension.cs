using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Rentifyx.Clients.Domain.Interfaces.Client;
using Rentifyx.Clients.Infrastructure.Repositories;

namespace Rentifyx.Clients.IoC;

public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IClientWriteOnlyRepository, ClientRepository>();
    }   
}
