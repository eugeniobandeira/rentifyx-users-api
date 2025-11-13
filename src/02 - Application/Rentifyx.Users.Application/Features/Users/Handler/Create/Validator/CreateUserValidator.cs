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
    private const int ZipCodeLength = 8;
    private const int StateLength = 2;
    private const int MaxStreetLength = 200;
    private const int MaxNumberLength = 10;
    private const int MaxNeighborhoodLength = 100;
    private const int MaxCityLength = 100;
    private const int MaxComplementLength = 200;

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

        When(x => x.AddressRequestDto != null, () =>
        {
            RuleFor(x => x.AddressRequestDto.Street.Trim())
                .NotEmpty()
                .WithMessage(UserValidatorErrorMessageResource.EMPTY_STREET)
                .MaximumLength(MaxStreetLength)
                .WithMessage(UserValidatorErrorMessageResource.STREET_MAX_LENGTH);

            RuleFor(x => x.AddressRequestDto.Number.Trim())
                .NotEmpty()
                .WithMessage(UserValidatorErrorMessageResource.EMPTY_NUMBER)
                .MaximumLength(MaxNumberLength)
                .WithMessage(UserValidatorErrorMessageResource.NUMBER_MAX_LENGTH);

            RuleFor(x => x.AddressRequestDto.Neighborhood.Trim())
                .NotEmpty()
                .WithMessage(UserValidatorErrorMessageResource.EMPTY_NEIGHBORHOOD)
                .MaximumLength(MaxNeighborhoodLength)
                .WithMessage(UserValidatorErrorMessageResource.NEIGHBORHOOD_MAX_LENGTH);

            RuleFor(x => x.AddressRequestDto.City.Trim())
                .NotEmpty()
                .WithMessage(UserValidatorErrorMessageResource.EMPTY_CITY)
                .MaximumLength(MaxCityLength)
                .WithMessage(UserValidatorErrorMessageResource.CITY_MAX_LENGTH);

            RuleFor(x => x.AddressRequestDto.State.Trim())
                .NotEmpty()
                .WithMessage(UserValidatorErrorMessageResource.EMPTY_STATE)
                .Length(StateLength)
                .WithMessage(UserValidatorErrorMessageResource.STATE_LENGTH);

            RuleFor(x => x.AddressRequestDto.ZipCode.Trim())
                .NotEmpty()
                .WithMessage(UserValidatorErrorMessageResource.EMPTY_ZIPCODE)
                .Length(ZipCodeLength)
                .WithMessage(UserValidatorErrorMessageResource.ZIPCODE_LENGTH);

            RuleFor(x => x.AddressRequestDto.Complement)
                .MaximumLength(MaxComplementLength)
                .When(x => !string.IsNullOrWhiteSpace(x.AddressRequestDto.Complement))
                .WithMessage(UserValidatorErrorMessageResource.COMPLEMENT_MAX_LENGTH);
        });
    }

    private static bool IsDocumentValid(string document)
    {
        if (string.IsNullOrWhiteSpace(document))
            return false;

        var documentLength = document.Trim().Length;

        return (documentLength is CpfLength or CnpjLength);
    }
}
