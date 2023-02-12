using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Profile.Patient;

public class CreatePatientProfileReceptionistDto
{
    // TODO Recheck
    public byte[]? Photo { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
}