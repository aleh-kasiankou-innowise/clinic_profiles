using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Doctor;

namespace Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;

public interface IDoctorService
{
    Task<Guid> CreateProfileAsync(DoctorProfileDto newProfile);
    Task<ViewDoctorProfileDto> GetProfileAsync(Guid doctorId);
    Task<IEnumerable<DoctorInfoDto>> GetListingAsync();

    Task<DoctorInfoDto> GetPublicInfo(Guid id);
    Task<IEnumerable<DoctorInfoReceptionistDto>> GetListingForReceptionistAsync();
    Task UpdateProfileAsync(Guid doctorId, DoctorProfileUpdateDto updatedProfile);
    Task UpdateStatusAsync(Guid doctorId, Guid newStatusId);
}