using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;
using System.Net;

namespace Rentifyx.Users.Infrastructure.Repositories;

public sealed class UserRepository(
    IDynamoDBContext dynamoDBContext)
    : IAddOnlyUserRepository<UserEntity>
{
    private readonly IDynamoDBContext _dynamoDBContext = dynamoDBContext;

    public async Task AddAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        await _dynamoDBContext.SaveAsync(entity, cancellationToken);
    }
}
