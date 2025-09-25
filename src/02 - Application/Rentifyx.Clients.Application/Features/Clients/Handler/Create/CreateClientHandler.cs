using Microsoft.Extensions.Logging;
using Rentifyx.Clients.Application.Adapter;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Validator;
using Rentifyx.Clients.Domain.Entities;
using Rentifyx.Clients.Domain.Interfaces.Client;
using Rentifyx.Clients.Exceptions.ExceptionBase;
using System.Text.Json;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create;

public sealed class CreateClientHandler(
    IClientWriteOnlyRepository writeOnlyRepository) 
    : ICreateClientHandler
{
    private readonly IClientWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;

    public async Task<ClientEntity> RegisterClientAsync(
        CreateClientRequestDto request, 
        CancellationToken cancellationToken = default)
    {
        Validate(request);

        var dto = ClientAdapter.FromRequestToEntity(request);

        await _writeOnlyRepository.PutItemAsync(dto, cancellationToken);

        return dto;
    }

    private static void Validate(CreateClientRequestDto request)
    {
        var validator = new CreateClientValidator().Validate(request);

        if (!validator.IsValid)
        {
            var errors = validator.Errors
                .Select(error => error.ErrorMessage)
                .ToList().AsReadOnly();

            throw new ErrorOnValidationException(errors);
        }
    }
}
