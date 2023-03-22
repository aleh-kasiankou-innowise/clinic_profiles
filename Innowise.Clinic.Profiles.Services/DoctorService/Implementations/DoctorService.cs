using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.Dto.RabbitMq;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters.Abstractions;
using Innowise.Clinic.Profiles.Services.RabbitMqService.RabbitMqPublisher;
using Innowise.Clinic.Profiles.Services.Utilities.MappingService;
using Innowise.Clinic.Profiles.Specifications;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.Dto;

namespace Innowise.Clinic.Profiles.Services.DoctorService.Implementations;

public class DoctorService : IDoctorService
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher;
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository, IRabbitMqPublisher rabbitMqPublisher)
    {
        _doctorRepository = doctorRepository;
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public async Task<Guid> CreateProfileAsync(DoctorProfileDto newProfile)
    {
        var newDoctor = newProfile.CreateNewDoctorEntity();
        await _doctorRepository.CreateProfileAsync(newDoctor);

        var userCreationRequest =
            new AccountGenerationDto(newDoctor.Person.PersonId, UserRoles.Doctor, newDoctor.Email);
        _rabbitMqPublisher.SendAccountGenerationTask(userCreationRequest);
        return newDoctor.Person.PersonId;
    }

    public async Task<InternalClinicDoctorProfileDto> GetProfileAsync(Guid doctorId)
    {
        var doctor = await _doctorRepository.GetProfileAsync(doctorId);
        return doctor.ToInternalClinicProfileDto();
    }

    public async Task<IEnumerable<DoctorPublicInfoDto>> GetListingAsync(int page, int quantity,
        ICollectionFilter<Doctor> doctorFilter)
    {
        var filterWithLimitationToActiveDoctors = doctorFilter.ToFiltrationExpression()
            .And(Doctor.IsActive);

        var doctorsListing =
            await _doctorRepository.GetDoctorListingAsync(page, quantity, filterWithLimitationToActiveDoctors);
        return doctorsListing.ToPublicProfileDtoListing();
    }

    public async Task<DoctorPublicInfoDto> GetPublicInfo(Guid id)
    {
        var doctor = await _doctorRepository.GetProfileAsync(id);
        return doctor.ToPublicProfileDto();
    }

    public async Task<IEnumerable<DoctorInfoReceptionistDto>> GetListingForReceptionistAsync(int offset, int quantity)
    {
        var doctorListing = await _doctorRepository.GetDoctorListingAsync(offset, quantity);
        return doctorListing.ToReceptionistViewProfileDtoListing();
    }

    public async Task UpdateProfileAsync(Guid doctorId, DoctorProfileUpdateDto updatedProfile)
    {
        var doctor = await _doctorRepository.GetProfileAsync(doctorId);
        doctor.UpdateProfile(updatedProfile);

        await UpdateStatusAsyncWithoutSaving(doctor, updatedProfile.StatusId);
        await _doctorRepository.UpdateProfileAsync(doctor);
    }

    public async Task UpdateStatusAsync(Guid doctorId, Guid newStatusId)
    {
        var doctor = await _doctorRepository.GetProfileAsync(doctorId);
        await UpdateStatusAsyncWithoutSaving(doctor, newStatusId);
        await _doctorRepository.UpdateProfileAsync(doctor);
    }

    private async Task UpdateStatusAsyncWithoutSaving(Doctor doctor, Guid newStatusId)
    {
        var newStatus = await DoctorStatusCacheService.GetDoctorStatus(newStatusId, _doctorRepository);
        if (doctor.Status.IsActiveAccount != newStatus.IsActiveAccount)
        {
            var accountId = doctor.Person.UserId ??
                            throw new AccountNotLinkedException(
                                $"Profile {doctor.Person.PersonId} is not linked to the account.");
            _rabbitMqPublisher.ChangeDoctorAccountStatus(new AccountStatusChangeDto(accountId,
                newStatus.IsActiveAccount));
        }

        doctor.StatusId = newStatusId;
    }
}