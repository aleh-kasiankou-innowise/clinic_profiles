using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Services.FiltrationService;
using Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Innowise.Clinic.Profiles.Services.Utilities.MappingService;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.PatientService.Implementations;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IBus _bus;
    private readonly FilterResolver<Patient> _filterResolver;

    public PatientService(IPatientRepository patientRepository, IBus bus, FilterResolver<Patient> filterResolver)
    {
        _patientRepository = patientRepository;
        _bus = bus;
        _filterResolver = filterResolver;
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
        patient.UpdateProfile(updatedProfile);
        await _patientRepository.UpdateProfileAsync(patient);
    }

    public async Task DeleteProfileAsync(Guid patientId)
    {
        await _patientRepository.DeleteProfileAsync(patientId);
    }
}