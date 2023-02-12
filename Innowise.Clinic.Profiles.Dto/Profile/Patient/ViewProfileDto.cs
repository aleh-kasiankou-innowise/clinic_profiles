using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Profile.Patient;

public class ViewPatientProfileDto
{
    public Guid PatientId { get; set; }
    public byte[]? Photo { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    public string? MiddleName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}