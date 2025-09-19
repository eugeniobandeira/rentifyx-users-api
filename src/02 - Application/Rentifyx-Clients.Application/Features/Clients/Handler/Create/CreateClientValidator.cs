using FluentValidation;
using Rentifyx.Clients.Application.Features.Clients.Request;
using Rentifyx.Clients.Domain.MessageResource;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create;

public sealed class CreateClientValidator : AbstractValidator<CreateClientRequest>
{
    private const int CpfLength = 11;
    private const int CnpjLength = 14;
    private const int MinNameLength = 7;
    private const int MaxNameLength = 100;
    private const int MaxEmailLength = 100;

    public CreateClientValidator()
    {
        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage(ClientValidatorMessageResource.EMPTY_DOCUMENT)
            .Must(IsDocumentValid)
            .WithMessage(ClientValidatorMessageResource.DOCUMENT_LENGTH);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ClientValidatorMessageResource.EMPTY_NAME)
            .MinimumLength(MinNameLength)
            .WithMessage(ClientValidatorMessageResource.NAME_MIN_LENGTH)
            .MaximumLength(MaxNameLength)
            .WithMessage(ClientValidatorMessageResource.NAME_MAX_LENGTH);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ClientValidatorMessageResource.EMPTY_EMAIL)
            .EmailAddress()
            .WithMessage(ClientValidatorMessageResource.INVALID_EMAIL_ADDRESS)
            .MaximumLength(MaxEmailLength)
            .WithMessage(ClientValidatorMessageResource.INVALID_EMAIL_LENGTH);
    }

    private bool IsDocumentValid(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            return false;

        var documentLength = document.Trim().Length;

        return (documentLength == CpfLength || documentLength == CnpjLength);
    }
}
