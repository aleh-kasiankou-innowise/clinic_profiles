using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Receptionist;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Services.BlobService.Interfaces;
using Innowise.Clinic.Profiles.Services.RabbitMqService.RabbitMqPublisher;
using Innowise.Clinic.Profiles.Services.ReceptionistService.Interfaces;
using Innowise.Clinic.Profiles.Services.Utilities.MappingService;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.Dto;

namespace Innowise.Clinic.Profiles.Services.ReceptionistService.Implementations;

public class ReceptionistService : IReceptionistService
{
    private readonly IRabbitMqPublisher _authenticationServiceConnection;
    private readonly IReceptionistRepository _receptionistRepository;
    private readonly IBlobService _blobService;

    public ReceptionistService(IRabbitMqPublisher authenticationServiceConnection,
        IReceptionistRepository receptionistRepository, IBlobService blobService)
    {
        _authenticationServiceConnection = authenticationServiceConnection;
        _receptionistRepository = receptionistRepository;
        _blobService = blobService;
    }

    public async Task<Guid> CreateProfileAsync(CreateReceptionistProfileDto newProfile)
    {
        var newReceptionist = newProfile.CreateNewProfile(null);
        await _receptionistRepository.CreateProfileAsync(newReceptionist);
        var userCreationRequest =
            new AccountGenerationDto(newReceptionist.Person.PersonId, UserRoles.Receptionist, newReceptionist.Email);

        _authenticationServiceConnection.SendAccountGenerationTask(userCreationRequest);
        var photoUrl = await _blobService.SavePhotoAsync(newReceptionist.PersonId, newProfile.Photo);
        newReceptionist.Person.Photo = photoUrl;
        await _receptionistRepository.UpdateProfileAsync(newReceptionist);
        return newReceptionist.Person.PersonId;
    }

    public async Task<ViewReceptionistProfileDto> GetProfileAsync(Guid receptionistId)
    {
        var receptionist = await _receptionistRepository.GetProfileAsync(receptionistId);
        return receptionist.ToProfileDto();
    }

    public async Task<IEnumerable<ReceptionistInfoDto>> GetListingAsync(int page, int quantity)
    {
        var receptionistListing = await _receptionistRepository.GetListingAsync(page, quantity);
        return receptionistListing.ToReceptionistDtoListing();
    }

    public async Task UpdateProfileAsync(Guid receptionistId, EditReceptionistProfileDto updatedProfile)
    {
        var receptionist = await _receptionistRepository.GetProfileAsync(receptionistId);
        var photoUrl = await HandlePhotoUpdate(receptionistId, receptionist.Person.Photo, updatedProfile);
        receptionist.OfficeId = updatedProfile.OfficeId;
        receptionist.Person.FirstName = updatedProfile.FirstName;
        receptionist.Person.LastName = updatedProfile.LastName;
        receptionist.Person.MiddleName = updatedProfile.MiddleName;
        receptionist.Person.Photo = photoUrl;
    }

    public async Task DeleteProfileAsync(Guid receptionistId)
    {
        var receptionist = await _receptionistRepository.GetProfileAsync(receptionistId);
        if (receptionist.Person.UserId != null)
        {
            var accountId = (Guid)receptionist.Person.UserId;
            _authenticationServiceConnection.RemoveReceptionistAccount(accountId);
        }
        else
        {
            throw new InconsistentDataException("The receptionist is not linked to the account.");
        }
    }

    // TODO REMOVE DUPLICATION
    private async Task<string?> HandlePhotoUpdate(Guid fileId, string? savedPhoto,
        EditReceptionistProfileDto updatedProfile)
    {
        if (updatedProfile is { IsToDeletePhoto: false, Photo: not null })
        {
            return await _blobService.SavePhotoAsync(fileId, updatedProfile.Photo);
        }

        if (updatedProfile.IsToDeletePhoto && savedPhoto is not null)
        {
            await _blobService.DeletePhotoAsync(savedPhoto);
        }

        return savedPhoto;
    }
}