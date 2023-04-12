using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Dto.Profile.Receptionist;

public class CreateReceptionistProfileDto
{
    public IFormFile? Photo { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    public Guid OfficeId { get; set; }
}