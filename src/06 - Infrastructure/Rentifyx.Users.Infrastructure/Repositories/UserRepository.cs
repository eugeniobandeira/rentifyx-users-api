using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;
using System.Net;

namespace Rentifyx.Users.Infrastructure.Repositories;

public sealed class UserRepository(
    IAmazonDynamoDB amazonDynamoDB) 
    : IAddOnlyRepository<UserEntity>
{
    private readonly IAmazonDynamoDB _dynamoDb = amazonDynamoDB;

    public async Task<bool> AddAsync(
        UserEntity clientEntity, 
        string tableName, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var item = new Dictionary<string, AttributeValue>
            {
                { "Document", new AttributeValue { S = clientEntity.Document } },
                { "Name", new AttributeValue { S = clientEntity.Name } },
                { "EmailAddress", new AttributeValue { S = clientEntity.Email } }
            };

            var request = new PutItemRequest()
            {
                TableName = tableName,
                Item = item
            };

            var response = await _dynamoDb.PutItemAsync(request, cancellationToken);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception {ex.Message}");
            throw;
        }
    }
}
