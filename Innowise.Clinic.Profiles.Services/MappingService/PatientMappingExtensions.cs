using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Services.MappingService;

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

    public static Patient CreateNewPatientEntity(this PatientProfileWithNumberAndPhotoDto newProfile, Guid associatedUserId)
    {
        var newPerson = new Person
        {
            Photo = newProfile.Photo,
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
        return patients.Select(x => new PatientInfoDto
        {
            PatientId = x.Person.PersonId,
            FirstName = x.Person.FirstName,
            LastName = x.Person.LastName,
            MiddleName = x.Person.MiddleName,
            PhoneNumber = x.PhoneNumber
        });
    }

    public static Patient UpdateProfile(this Patient patient, PatientProfileWithNumberAndPhotoDto updatedProfile)
    {
        patient.Person.FirstName = updatedProfile.FirstName;
        patient.Person.LastName = updatedProfile.LastName;
        patient.Person.MiddleName = updatedProfile.MiddleName;
        patient.Person.Photo = updatedProfile.Photo ?? patient.Person.Photo;
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