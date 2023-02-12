using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Profile.Receptionist;

public class ViewReceptionistProfileDto
{
    public Guid ReceptionistId { get; set; }
    public byte[]? Photo { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    public string? MiddleName { get; set; }
    public Guid OfficeId { get; set; }
}