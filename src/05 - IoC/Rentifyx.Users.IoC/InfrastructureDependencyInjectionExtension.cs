using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.DependencyInjection;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;
using Rentifyx.Users.Infrastructure.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Rentifyx.Users.IoC;

[ExcludeFromCodeCoverage]
public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddScoped<IDynamoDBContext, DynamoDBContext>();
        services.AddDefaultAWSOptions(new AWSOptions
        {
            Region = RegionEndpoint.SAEast1 
        });
        services.AddScoped<IAddOnlyUserRepository<UserEntity>, UserRepository>();
    }   
}
