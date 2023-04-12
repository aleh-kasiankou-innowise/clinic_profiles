using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Services.Utilities.MappingService;

public static class DoctorMappingExtensions
{
    public static Doctor CreateNewDoctorEntity(this DoctorProfileDto doctorProfileDto, string? photo)
    {
        var newPerson = new Person
        {
            FirstName = doctorProfileDto.FirstName,
            LastName = doctorProfileDto.LastName,
            MiddleName = doctorProfileDto.MiddleName,
            Photo = photo
        };

        var newDoctor = new Doctor
        {
            Person = newPerson,
            CareerStartYear = doctorProfileDto.CareerStartYear,
            Email = doctorProfileDto.Email,
            OfficeId = doctorProfileDto.OfficeId,
            SpecializationId = doctorProfileDto.SpecializationId,
            StatusId = doctorProfileDto.StatusId
        };

        return newDoctor;
    }

    public static InternalClinicDoctorProfileDto ToInternalClinicProfileDto(this Doctor doctor)
    {
        return new InternalClinicDoctorProfileDto
        {
            DoctorId = doctor.Person.PersonId,
            CareerStartYear = doctor.CareerStartYear,
            StatusId = doctor.StatusId,
            DateOfBirth = doctor.DateOfBirth,
            Email = doctor.Email,
            FirstName = doctor.Person.FirstName,
            LastName = doctor.Person.LastName,
            MiddleName = doctor.Person.MiddleName,
            OfficeId = doctor.OfficeId,
            SpecializationId = doctor.SpecializationId,
            Photo = doctor.Person.Photo
        };
    }

    public static IEnumerable<DoctorPublicInfoDto> ToPublicProfileDtoListing(this IEnumerable<Doctor> doctors)
    {
        return doctors.Select(d => d.ToPublicProfileDto());
    }

    public static DoctorPublicInfoDto ToPublicProfileDto(this Doctor doctor)
    {
        return new DoctorPublicInfoDto(
            doctor.DoctorId,
            doctor.Person.FirstName,
            doctor.Person.LastName,
            doctor.Person.MiddleName,
            doctor.SpecializationId,
            doctor.OfficeId,
            doctor.CareerStartYear,
            doctor.Person.Photo
        );
    }

    public static IEnumerable<DoctorInfoReceptionistDto> ToReceptionistViewProfileDtoListing(this IEnumerable<Doctor> doctors)
    {
        return doctors.Select(d =>
            new DoctorInfoReceptionistDto(d.DoctorId, d.Person.FirstName, d.Person.LastName, d.Person.MiddleName,
                d.SpecializationId, d.OfficeId, d.StatusId));
    }

    public static Doctor UpdateProfile(this Doctor doctor, DoctorProfileUpdateDto updatedProfile, string? photo)
    {
        doctor.Person.FirstName = updatedProfile.FirstName;
        doctor.Person.LastName = updatedProfile.LastName;
        doctor.Person.MiddleName = updatedProfile.MiddleName;
        doctor.DateOfBirth = updatedProfile.DateOfBirth;
        doctor.Person.Photo = photo;
        doctor.SpecializationId = updatedProfile.SpecializationId;
        doctor.CareerStartYear = updatedProfile.CareerStartYear;

        return doctor;
    }
}