using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Dto.Profile.Receptionist;
using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Services.Utilities.MappingService;

public static class ReceptionistMappingService
{
    public static Receptionist CreateNewProfile(this CreateReceptionistProfileDto newProfile, string? photo)
    {
        var newPerson = new Person
        {
            FirstName = newProfile.FirstName,
            LastName = newProfile.LastName,
            MiddleName = newProfile.MiddleName,
            Photo = photo
        };

        var newReceptionist = new Receptionist
        {
            Email = newProfile.Email,
            OfficeId = newProfile.OfficeId,
            Person = newPerson
        };

        return newReceptionist;
    }

    public static ViewReceptionistProfileDto ToProfileDto(this Receptionist receptionist)
    {
        return new ViewReceptionistProfileDto
        {
            ReceptionistId = receptionist.Person.PersonId,
            FirstName = receptionist.Person.FirstName,
            LastName = receptionist.Person.LastName,
            MiddleName = receptionist.Person.MiddleName,
            OfficeId = receptionist.OfficeId,
            Photo = receptionist.Person.Photo
        };
    }

    public static IEnumerable<ReceptionistInfoDto> ToReceptionistDtoListing(
        this IEnumerable<Receptionist> receptionists)
    {
        return receptionists.Select(r =>
            new ReceptionistInfoDto(r.PersonId, r.Person.FirstName, r.Person.LastName, r.OfficeId,
                r.Person.MiddleName));
    }
}