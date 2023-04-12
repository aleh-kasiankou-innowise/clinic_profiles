using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Services.Utilities.MappingService;

public static class PatientMappingExtensions
{
    public static Patient CreateNewPatientEntity(this PatientProfileDto newProfile)
    {
        var newPerson = new Person
        {
            FirstName = newProfile.FirstName,
            LastName = newProfile.LastName,
            MiddleName = newProfile.MiddleName
        };

        var newPatientProfile = new Patient
        {
            Person = newPerson,
            DateOfBirth = newProfile.DateOfBirth
        };

        return newPatientProfile;
    }

    public static Patient CreateNewPatientEntity(this PatientProfileWithNumberAndPhotoDto newProfile, Guid associatedUserId, string? photo)
    {
        var newPerson = new Person
        {
            Photo = photo,
            FirstName = newProfile.FirstName,
            LastName = newProfile.LastName,
            MiddleName = newProfile.MiddleName,
            UserId = associatedUserId
        };

        var newPatientProfile = new Patient
        {
            Person = newPerson,
            PhoneNumber = newProfile.PhoneNumber,
            DateOfBirth = newProfile.DateOfBirth
        };

        return newPatientProfile;
    }

    public static IEnumerable<PatientInfoDto> ToPatientInfoDtoListing(this IEnumerable<Patient> patients)
    {
        return patients.Select(x =>
            new PatientInfoDto(x.PersonId, x.Person.FirstName, x.Person.LastName, x.PhoneNumber, x.Person.MiddleName));
    }

    public static Patient UpdateProfile(this Patient patient, PatientProfileWithNumberAndPhotoDto updatedProfile, string? photo)
    {
        patient.Person.FirstName = updatedProfile.FirstName;
        patient.Person.LastName = updatedProfile.LastName;
        patient.Person.MiddleName = updatedProfile.MiddleName;
        patient.Person.Photo = photo;
        patient.DateOfBirth = updatedProfile.DateOfBirth;
        patient.PhoneNumber = updatedProfile.PhoneNumber;
        return patient;
    }

    public static ViewPatientProfileDto ToPatientProfileDto(this Patient patient)
    {
        return new ViewPatientProfileDto
        {
            PatientId = patient.Person.PersonId,
            FirstName = patient.Person.FirstName,
            LastName = patient.Person.LastName,
            MiddleName = patient.Person.MiddleName,
            DateOfBirth = patient.DateOfBirth,
            PhoneNumber = patient.PhoneNumber,
            Photo = patient.Person.Photo
        };
    }
}