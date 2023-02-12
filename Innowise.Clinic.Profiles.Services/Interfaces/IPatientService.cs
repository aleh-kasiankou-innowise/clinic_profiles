using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;

namespace Innowise.Clinic.Profiles.Services.Interfaces;

public interface IPatientService
{
    Task<Guid> CreateProfileAsync(CreateEditPatientProfileDto newProfile);
    Task<Guid> CreateProfileAsync(CreatePatientProfileReceptionistDto newProfile);

    Task<ViewPatientProfileDto> GetPatientProfileAsync(Guid patientId);
    Task<IEnumerable<PatientInfoDto>> GetPatientListingAsync();
    Task UpdateProfileAsync(Guid patientId, CreateEditPatientProfileDto updatedProfile);
    Task DeleteProfileAsync(Guid patientId);
}