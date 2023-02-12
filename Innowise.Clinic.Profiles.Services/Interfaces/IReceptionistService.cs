using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Receptionist;

namespace Innowise.Clinic.Profiles.Services.Interfaces;

public interface IReceptionistService
{
    Task<Guid> CreateProfileAsync(CreateReceptionistProfileDto newProfile);
    Task<ViewReceptionistProfileDto> GetProfileAsync(Guid patientId);
    Task<IEnumerable<PatientInfoDto>> GetListingAsync();
    Task UpdateProfileAsync(Guid receptionistId, EditReceptionistProfileDto updatedProfile);
    Task DeleteProfileAsync(Guid receptionistId);
}