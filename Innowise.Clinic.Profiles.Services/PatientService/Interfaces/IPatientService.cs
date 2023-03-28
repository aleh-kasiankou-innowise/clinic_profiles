using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;

namespace Innowise.Clinic.Profiles.Services.PatientService.Interfaces;

public interface IPatientService
{
    Task<Guid> CreateProfileAsync(PatientProfileDto newProfile);
    Task<Guid> CreateProfileAsync(PatientProfileWithNumberAndPhotoDto newProfile, Guid associatedUserId);

    Task<ViewPatientProfileDto> GetPatientProfileAsync(Guid patientId);
    Task<IEnumerable<PatientInfoDto>> GetPatientListingAsync(int page, int quantity,
        CompoundFilter<Patient> compoundFilter);
    Task UpdateProfileAsync(Guid patientId, PatientProfileWithNumberAndPhotoDto updatedProfile);
    Task DeleteProfileAsync(Guid patientId);
}