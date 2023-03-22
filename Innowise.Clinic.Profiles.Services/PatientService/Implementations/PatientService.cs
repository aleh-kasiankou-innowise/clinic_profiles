using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Innowise.Clinic.Profiles.Services.Utilities.MappingService;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.PatientService.Implementations;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IBus _bus;

    public PatientService(IPatientRepository patientRepository, IBus bus)
    {
        _patientRepository = patientRepository;
        _bus = bus;
    }

    public async Task<Guid> CreateProfileAsync(PatientProfileDto newProfile)
    {
        var newPatientProfile = newProfile.CreateNewPatientEntity();
        await _patientRepository.CreateProfileAsync(newPatientProfile);
        return newPatientProfile.Person.PersonId;
    }

    public async Task<Guid> CreateProfileAsync(PatientProfileWithNumberAndPhotoDto newProfile, Guid associatedUserId)
    {
        var newPatientProfile = newProfile.CreateNewPatientEntity(associatedUserId);
        await _patientRepository.CreateProfileAsync(newPatientProfile);
        await _bus.Publish(new PatientCreatedProfileMessage(newPatientProfile.Person.PersonId, associatedUserId));
        return newPatientProfile.Person.PersonId;
    }

    public async Task<ViewPatientProfileDto> GetPatientProfileAsync(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientProfileAsync(patientId);
        return patient.ToPatientProfileDto();
    }

    public async Task<IEnumerable<PatientInfoDto>> GetPatientListingAsync(int page, int quantity)
    {
        var patientListing = await _patientRepository.GetPatientListingAsync(page, quantity);
        return patientListing.ToPatientInfoDtoListing();
    }

    public async Task UpdateProfileAsync(Guid patientId, PatientProfileWithNumberAndPhotoDto updatedProfile)
    {
        var patient = await _patientRepository.GetPatientProfileAsync(patientId);
        patient.UpdateProfile(updatedProfile);
        await _patientRepository.UpdateProfileAsync(patient);
    }

    public async Task DeleteProfileAsync(Guid patientId)
    {
        await _patientRepository.DeleteProfileAsync(patientId);
    }
}