using ErrorOr;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace Rentifyx.Users.Application.Features.Users.Handler.GetByDocument;

public class GetUserByDocumentHandler(IReadOnlyUserRepository readOnlyUserRepository) 
    : IGetUserByDocumentHandler
{
    private readonly IReadOnlyUserRepository _readOnlyUserRepository = readOnlyUserRepository;

    private const int _minDocumentLength = 11;
    private const int _maxDocumentLength = 14;

    public async Task<ErrorOr<UserEntity>> GetUserByDocumentAsync(
        string document, 
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(document))
        {
            return Error.Validation(
                code: "User.Document.Empty",
                description: "The document cannot be empty.");
        }

        if (document.Length is not (_minDocumentLength or _maxDocumentLength))
            {
            return Error.Validation(
                code: "User.Document.InvalidFormat",
                description: "The document must have 11 or 14 characters.");
        }


        return await _readOnlyUserRepository.GetByDocumentAsync(document, cancellationToken);
    }
}
