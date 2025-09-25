using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Rentifyx.Clients.Domain.Entities;
using Rentifyx.Clients.Domain.Interfaces.Client;
using System.Net;

namespace Rentifyx.Clients.Infrastructure.Repositories;

public sealed class ClientRepository : IClientWriteOnlyRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName;

    public ClientRepository(
        IAmazonDynamoDB amazonDynamoDB,
        string tableName = "rentifyx-clients-dev")
    {
        _dynamoDb = amazonDynamoDB;
        _tableName = tableName;
    }

    public async Task<bool> PutItemAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(clientEntity);

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
                TableName = _tableName,
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
