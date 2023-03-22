using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;

namespace Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;

public interface IDoctorService
{
    Task<Guid> CreateProfileAsync(DoctorProfileDto newProfile);
    Task<InternalClinicDoctorProfileDto> GetProfileAsync(Guid doctorId);
    Task<IEnumerable<DoctorPublicInfoDto>> GetListingAsync(int page, int quantity, ICompoundFilter<Doctor> filter);
    Task<DoctorPublicInfoDto> GetPublicInfo(Guid id);
    Task<IEnumerable<DoctorInfoReceptionistDto>> GetListingForReceptionistAsync(int page, int quantity,
        CompoundFilter<Doctor> compoundFilter);
    Task UpdateProfileAsync(Guid doctorId, DoctorProfileUpdateDto updatedProfile);
    Task UpdateStatusAsync(Guid doctorId, Guid newStatusId);
}