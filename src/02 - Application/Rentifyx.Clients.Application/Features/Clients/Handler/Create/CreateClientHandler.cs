using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Rentifyx.Clients.Application.Adapter;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Validator;
using Rentifyx.Clients.Domain.Entities;
using Rentifyx.Clients.Domain.Interfaces.Client;
using Rentifyx.Clients.Domain.Shared.Results;
using Rentifyx.Clients.Exceptions.ExceptionBase;
using System.Text.Json;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create;

public sealed class CreateClientHandler(
    IValidator<CreateClientRequestDto> validator,
    IClientWriteOnlyRepository writeOnlyRepository) 
    : ICreateClientHandler
{
    private readonly IClientWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IValidator<CreateClientRequestDto> _validator = validator;

    public async Task<ErrorOr<ClientEntity>> RegisterClientAsync(
        CreateClientRequestDto request, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .ConvertAll(error => Error.Validation(
                    code: error.PropertyName,
                    description: error.ErrorMessage));

            return errors;
        }
            

        var client = ClientAdapter.FromRequestToEntity(request);

        await _writeOnlyRepository.AddAsync(client, cancellationToken);

        return client;
    }
}
