using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Listing;

public class PatientInfoDto
{
    public Guid PatientId { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    public string? MiddleName { get; set; }
    public string PhoneNumber { get; set; }
}