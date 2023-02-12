using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Listing;

public class DoctorInfoDto
{
    public Guid DoctorId { get; set; }
    public byte[]? Photo { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    public string? MiddleName { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public DateTime CareerStartYear { get; set; }
}