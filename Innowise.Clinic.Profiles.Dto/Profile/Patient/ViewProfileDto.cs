using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Dto.Profile.Patient;

public class ViewPatientProfileDto
{
    public string? Photo { get; set; }
    public Guid PatientId { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}