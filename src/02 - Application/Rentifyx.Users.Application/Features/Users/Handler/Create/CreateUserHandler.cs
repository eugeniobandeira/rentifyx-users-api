using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rentifyx.Users.Application.Adapter;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace Rentifyx.Users.Application.Features.Users.Handler.Create;

public sealed class CreateUserHandler(
    IValidator<CreateUserRequestDto> validator,
    IAddOnlyUserRepository<UserEntity> addOnlyRepository,
    IReadOnlyUserRepository readOnlyUserRepository,
    ILogger<CreateUserHandler> logger)
    : ICreateUserHandler
{
    private readonly IAddOnlyUserRepository<UserEntity> _addOnlyRepository = addOnlyRepository;
    private readonly IReadOnlyUserRepository _readOnlyUserRepository = readOnlyUserRepository;
    private readonly IValidator<CreateUserRequestDto> _validator = validator;
    private readonly ILogger<CreateUserHandler> _logger = logger;

    public async Task<ErrorOr<UserEntity>> CreateUsersAsync(
        CreateUserRequestDto request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting client creation process. Request: {Request}", request);

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(error => Error.Validation(
                    code: error.PropertyName,
                    description: error.ErrorMessage))
                .ToList();

            _logger.LogError("Client creation failed due to validation errors: {Errors}", errors);

            return errors;
        }

        var user = UserAdapter.FromRequestToEntity(request);

        await _addOnlyRepository.AddAsync(user, cancellationToken);

        return user;
    }
}
