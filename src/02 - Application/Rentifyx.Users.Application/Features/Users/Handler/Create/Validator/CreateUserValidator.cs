using FluentValidation;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Exceptions.MessageResource;

namespace Rentifyx.Users.Application.Features.Users.Handler.Create.Validator;

public sealed class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
{
    private const int CpfLength = 11;
    private const int CnpjLength = 14;
    private const int MinNameLength = 7;
    private const int MaxNameLength = 100;

    public CreateUserValidator()
    {
        RuleFor(x => x.Document.Trim())
            .NotEmpty()
            .WithMessage(UserValidatorErrorMessageResource.EMPTY_DOCUMENT)
            .Must(IsDocumentValid)
            .WithMessage(UserValidatorErrorMessageResource.DOCUMENT_LENGTH);

        RuleFor(x => x.Name.Trim())
            .NotEmpty()
            .WithMessage(UserValidatorErrorMessageResource.EMPTY_NAME)
            .MinimumLength(MinNameLength)
            .WithMessage(UserValidatorErrorMessageResource.NAME_MIN_LENGTH)
            .MaximumLength(MaxNameLength)
            .WithMessage(UserValidatorErrorMessageResource.NAME_MAX_LENGTH);

        RuleFor(x => x.Email.Trim())
            .NotEmpty()
            .WithMessage(UserValidatorErrorMessageResource.EMPTY_EMAIL)
            .EmailAddress()
            .WithMessage(UserValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);
    }

    private static bool IsDocumentValid(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            return false;

        var documentLength = document.Trim().Length;

        return (documentLength is CpfLength or CnpjLength);
    }
}
