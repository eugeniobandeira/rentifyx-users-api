using FluentValidation;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Validator;

namespace CommomTestUtilities.Validator;

public static class CreateUserValidatorBuilder
{
    public static IValidator<CreateUserRequestDto> Build()
    {
        return new CreateUserValidator();
    }
}
