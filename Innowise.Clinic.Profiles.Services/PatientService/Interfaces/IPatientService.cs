using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;

namespace Innowise.Clinic.Profiles.Services.PatientService.Interfaces;

public interface IPatientService
{
    Task<Guid> CreateProfileAsync(PatientProfileDto newProfile);
    Task<Guid> CreateProfileAsync(PatientProfileWithNumberAndPhotoDto newProfile, Guid associatedUserId);

    Task<ViewPatientProfileDto> GetPatientProfileAsync(Guid patientId);
    Task<IEnumerable<PatientInfoDto>> GetPatientListingAsync();
    Task UpdateProfileAsync(Guid patientId, PatientProfileWithNumberAndPhotoDto updatedProfile);
    Task DeleteProfileAsync(Guid patientId);
}