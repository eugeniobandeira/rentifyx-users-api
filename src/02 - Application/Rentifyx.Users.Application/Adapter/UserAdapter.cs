using Rentifyx.Users.Application.Commom.Dto;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Domain.Constants;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.ValueObjects;

namespace Rentifyx.Users.Application.Adapter;

public static class UserAdapter
{
    public static UserEntity FromRequestToEntity(CreateUserRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.AddressRequestDto);

        var address = MapAddress(request.AddressRequestDto);
        var profileImage = MapProfileImage(request.ProfileImageFileName);

        return new UserEntity()
        {
            Name = request.Name,
            Document = request.Document,
            Email = request.Email,
            Address = address,
            ProfileImage = profileImage
        };
    }

    private static Address MapAddress(AddressRequestDto addressRequestDto)
    {
        ArgumentNullException.ThrowIfNull(addressRequestDto);

        return Address.Builder()
            .WithStreet(addressRequestDto.Street)
            .WithNumber(addressRequestDto.Number)
            .WithNeighborhood(addressRequestDto.Neighborhood)
            .WithCity(addressRequestDto.City)
            .WithState(addressRequestDto.State)
            .WithZipCode(addressRequestDto.ZipCode)
            .WithComplement(addressRequestDto.Complement)
            .Build();
    }

    private static ProfileImage? MapProfileImage(string? profileImageFileName)
    {
        if (string.IsNullOrWhiteSpace(profileImageFileName))
            return null;

        return ProfileImage.Create(
            profileImageFileName,
            AwsContants.RENTIFYX_PROFILE_IMAGES_BUCKET);
    }
}
