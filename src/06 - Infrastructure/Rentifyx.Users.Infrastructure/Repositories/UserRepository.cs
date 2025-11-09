using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace Rentifyx.Users.Infrastructure.Repositories;

public sealed class UserRepository(
    IDynamoDBContext dynamoDBContext,
    ILogger<UserRepository> logger)
    : IAddOnlyUserRepository<UserEntity>,
      IReadOnlyUserRepository
{
    private readonly IDynamoDBContext _dynamoDBContext = dynamoDBContext;
    private readonly ILogger<UserRepository> _logger = logger;

    public async Task<ErrorOr<UserEntity>> AddAsync(
        UserEntity entity, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dynamoDBContext.SaveAsync(entity, cancellationToken);

            _logger.LogInformation("User with document {Document} added successfully", entity.Document);

            return entity;
        }
        catch (AmazonDynamoDBException ex)
        {
            _logger.LogError(ex, "DynamoDB error adding user with document {Document}", entity.Document);

            return Error.Unexpected(
                code: "Repository.DynamoDBError",
                description: "A database error occurred while adding the user.");
        }
    }


    public async Task<ErrorOr<UserEntity>> GetByDocumentAsync(
        string document, 
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _dynamoDBContext.LoadAsync<UserEntity>(document, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User with document {Document} was not found", document);

                return Error.NotFound(
                    code: "User.NotFound",
                    description: $"User with document '{document}' was not found.");
            }

            _logger.LogDebug("User with document {Document} found successfully", document);

            return user;
        }
        catch (AmazonDynamoDBException ex)
        {
            _logger.LogError(ex, "Error retrieving user with document {Document}, error message {ErrorMessage}", document, ex.Message);

            return Error.Unexpected(
                code: "Repository.Error",
                description: "An error occurred while retrieving the user.");
        }
    }
}
