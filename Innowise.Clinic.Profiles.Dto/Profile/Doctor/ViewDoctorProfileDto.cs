using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Profile.Doctor;

public class ViewDoctorProfileDto
{
    public byte[]? Photo { get; set; }

    public Guid DoctorId { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public DateTime CareerStartYear { get; set; }
    public Guid StatusId { get; set; }
}