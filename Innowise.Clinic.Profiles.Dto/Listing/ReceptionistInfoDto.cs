using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Listing;

public class ReceptionistInfoDto
{
    public Guid ReceptionistId { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public Guid OfficeId { get; set; }
}