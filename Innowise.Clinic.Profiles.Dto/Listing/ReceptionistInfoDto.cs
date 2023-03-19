using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Listing;

public record ReceptionistInfoDto(Guid ReceptionistId, [Required] string FirstName, [Required] string LastName,
    Guid OfficeId, string? MiddleName = null);