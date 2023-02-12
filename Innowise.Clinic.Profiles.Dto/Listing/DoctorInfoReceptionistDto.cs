using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Listing;

public class DoctorInfoReceptionistDto
{
    public Guid DoctorId { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public Guid StatusId { get; set; }
}