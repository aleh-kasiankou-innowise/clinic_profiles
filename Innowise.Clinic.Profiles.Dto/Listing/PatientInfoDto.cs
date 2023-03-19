using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Listing;

public record PatientInfoDto(Guid PatientId, [Required] string FirstName, [Required] string LastName,
    string? PhoneNumber, string? MiddleName = null);