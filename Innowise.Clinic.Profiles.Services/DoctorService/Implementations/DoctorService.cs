using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.Dto.RabbitMq;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Profiles.Services.BlobService.Interfaces;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Innowise.Clinic.Profiles.Services.RabbitMqService.RabbitMqPublisher;
using Innowise.Clinic.Profiles.Services.Utilities.MappingService;
using Innowise.Clinic.Profiles.Specifications;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using Innowise.Clinic.Shared.Services.FiltrationService;
using Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.DoctorService.Implementations;

public class DoctorService : IDoctorService
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher;
    private readonly IDoctorRepository _doctorRepository;
    private readonly FilterResolver<Doctor> _filterResolver;
    private readonly IBlobService _blobService;
    private readonly IBus _bus;

    public DoctorService(IDoctorRepository doctorRepository, IRabbitMqPublisher rabbitMqPublisher,
        FilterResolver<Doctor> filterResolver, IBus bus, IBlobService blobService)
    {
        _doctorRepository = doctorRepository;
        _rabbitMqPublisher = rabbitMqPublisher;
        _filterResolver = filterResolver;
        _blobService = blobService;
        _bus = bus;
    }

    public async Task<Guid> CreateProfileAsync(DoctorProfileDto newProfile)
    {
        var photoUrl = await _blobService.UploadPhotoAsync(newProfile.Photo);
        var newDoctor = newProfile.CreateNewDoctorEntity(photoUrl);
        await _doctorRepository.CreateProfileAsync(newDoctor);
        _rabbitMqPublisher.SendAccountGenerationTask(new(newDoctor.Person.PersonId, UserRoles.Doctor, newDoctor.Email));
        await _bus.Publish<DoctorAddedOrUpdatedMessage>(new(newDoctor.DoctorId, newDoctor.SpecializationId,
            newDoctor.OfficeId));
        return newDoctor.Person.PersonId;
    }

    public async Task<InternalClinicDoctorProfileDto> GetProfileAsync(Guid doctorId)
    {
        var doctor = await _doctorRepository.GetProfileAsync(doctorId);
        return doctor.ToInternalClinicProfileDto();
    }

    public async Task<IEnumerable<DoctorPublicInfoDto>> GetListingAsync(int page, int quantity,
        ICompoundFilter<Doctor> doctorFilter)
    {
        var filterWithLimitationToActiveDoctors = _filterResolver.ConvertCompoundFilterToExpression(doctorFilter)
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

    public async Task<IEnumerable<DoctorInfoReceptionistDto>> GetListingForReceptionistAsync(int offset, int quantity,
        CompoundFilter<Doctor> compoundFilter)
    {
        var filter = _filterResolver.ConvertCompoundFilterToExpression(compoundFilter);
        var doctorListing = await _doctorRepository.GetDoctorListingAsync(offset, quantity, filter);
        return doctorListing.ToReceptionistViewProfileDtoListing();
    }

    public async Task UpdateProfileAsync(Guid doctorId, DoctorProfileUpdateDto updatedProfile)
    {
        var doctor = await _doctorRepository.GetProfileAsync(doctorId);
        var photoUrl = await HandlePhotoUpdate(doctor.Person.Photo, updatedProfile);
        doctor.UpdateProfile(updatedProfile, photoUrl);
        await UpdateStatusAsyncWithoutSaving(doctor, updatedProfile.StatusId);
        await _doctorRepository.UpdateProfileAsync(doctor);
        await _bus.Publish<DoctorAddedOrUpdatedMessage>(new(doctor.DoctorId, doctor.SpecializationId,
            doctor.OfficeId));
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

    private async Task<string?> HandlePhotoUpdate(string? savedPhoto, DoctorProfileUpdateDto updatedProfile)
    {
        var photoUrl = savedPhoto;

        if (updatedProfile.IsToDeletePhoto && savedPhoto is not null)
        {
            await _blobService.DeletePhotoAsync(savedPhoto);
            photoUrl = null;
        }

        else if (updatedProfile is { IsToDeletePhoto: false, Photo: not null })
        {
            if (savedPhoto is null)
            {
                photoUrl = await _blobService.UploadPhotoAsync(updatedProfile.Photo);
            }
            else
            {
                await _blobService.UpdatePhotoAsync(updatedProfile.Photo, savedPhoto);
            }
        }

        return photoUrl;
    }
}