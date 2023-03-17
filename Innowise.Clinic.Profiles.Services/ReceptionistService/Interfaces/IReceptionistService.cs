using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Receptionist;

namespace Innowise.Clinic.Profiles.Services.ReceptionistService.Interfaces;

public interface IReceptionistService
{
    Task<Guid> CreateProfileAsync(CreateReceptionistProfileDto newProfile);
    Task<ViewReceptionistProfileDto> GetProfileAsync(Guid receptionistId);
    Task<IEnumerable<ReceptionistInfoDto>> GetListingAsync(int page, int quantity);
    Task UpdateProfileAsync(Guid receptionistId, EditReceptionistProfileDto updatedProfile);
    Task DeleteProfileAsync(Guid receptionistId);
}