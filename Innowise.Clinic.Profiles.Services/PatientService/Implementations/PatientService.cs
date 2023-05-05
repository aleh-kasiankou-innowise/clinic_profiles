using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Services.BlobService.Interfaces;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Innowise.Clinic.Profiles.Services.Utilities.MappingService;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using Innowise.Clinic.Shared.Services.FiltrationService;
using Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.PatientService.Implementations;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly FilterResolver<Patient> _filterResolver;
    private readonly IBlobService _blobService;
    private readonly IBus _bus;

    public PatientService(IPatientRepository patientRepository, IBus bus, FilterResolver<Patient> filterResolver,
        IBlobService blobService)
    {
        _patientRepository = patientRepository;
        _bus = bus;
        _filterResolver = filterResolver;
        _blobService = blobService;
    }

    public async Task<Guid> CreateProfileAsync(PatientProfileDto newProfile)
    {
        var newPatientProfile = newProfile.CreateNewPatientEntity();
        await _patientRepository.CreateProfileAsync(newPatientProfile);
        return newPatientProfile.Person.PersonId;
    }

    public async Task<Guid> CreateProfileAsync(PatientProfileWithNumberAndPhotoDto newProfile, Guid associatedUserId)
    {
        var newPatientProfile = newProfile.CreateNewPatientEntity(associatedUserId, null);
        await _patientRepository.CreateProfileAsync(newPatientProfile);
        await _bus.Publish(new PatientCreatedProfileMessage(newPatientProfile.Person.PersonId, associatedUserId));
        var photoUrl = await _blobService.SavePhotoAsync(newPatientProfile.PersonId, newProfile.Photo);
        newPatientProfile.Person.Photo = photoUrl;
        await _patientRepository.UpdateProfileAsync(newPatientProfile);
        return newPatientProfile.Person.PersonId;
    }

    public async Task<ViewPatientProfileDto> GetPatientProfileAsync(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientProfileAsync(patientId);
        return patient.ToPatientProfileDto();
    }

    public async Task<IEnumerable<PatientInfoDto>> GetPatientListingAsync(int page, int quantity,
        CompoundFilter<Patient> compoundFilter)
    {
        var filter = _filterResolver.ConvertCompoundFilterToExpression(compoundFilter);
        var patientListing = await _patientRepository.GetPatientListingAsync(page, quantity, filter);
        return patientListing.ToPatientInfoDtoListing();
    }

    public async Task UpdateProfileAsync(Guid patientId, PatientProfileWithNumberAndPhotoDto updatedProfile)
    {
        var patient = await _patientRepository.GetPatientProfileAsync(patientId);
        var photoUrl = await HandlePhotoUpdate(patientId, patient.Person.Photo, updatedProfile);
        patient.UpdateProfile(updatedProfile, photoUrl);
        await _patientRepository.UpdateProfileAsync(patient);
    }

    public async Task DeleteProfileAsync(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientProfileAsync(patientId);
        if (patient.Person.Photo is not null)
        {
            await _blobService.DeletePhotoAsync(patient.Person.Photo);
        }
        await _patientRepository.DeleteProfileAsync(patient);
    }

    // TODO REMOVE DUPLICATION
    private async Task<string?> HandlePhotoUpdate(Guid fileId, string? savedPhoto,
        PatientProfileWithNumberAndPhotoDto updatedProfile)
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