using FluentValidation;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Exceptions.MessageResource;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create.Validator;

public sealed class CreateClientValidator : AbstractValidator<CreateClientRequestDto>
{
    private const int CpfLength = 11;
    private const int CnpjLength = 14;
    private const int MinNameLength = 7;
    private const int MaxNameLength = 100;

    public CreateClientValidator()
    {
        RuleFor(x => x.Document.Trim())
            .NotEmpty()
            .WithMessage(ClientValidatorErrorMessageResource.EMPTY_DOCUMENT)
            .Must(IsDocumentValid)
            .WithMessage(ClientValidatorErrorMessageResource.DOCUMENT_LENGTH);

        RuleFor(x => x.Name.Trim())
            .NotEmpty()
            .WithMessage(ClientValidatorErrorMessageResource.EMPTY_NAME)
            .MinimumLength(MinNameLength)
            .WithMessage(ClientValidatorErrorMessageResource.NAME_MIN_LENGTH)
            .MaximumLength(MaxNameLength)
            .WithMessage(ClientValidatorErrorMessageResource.NAME_MAX_LENGTH);

        RuleFor(x => x.Email.Trim())
            .NotEmpty()
            .WithMessage(ClientValidatorErrorMessageResource.EMPTY_EMAIL)
            .EmailAddress()
            .WithMessage(ClientValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);
    }

    private static bool IsDocumentValid(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            return false;

        var documentLength = document.Trim().Length;

        return (documentLength is CpfLength or CnpjLength);
    }
}
