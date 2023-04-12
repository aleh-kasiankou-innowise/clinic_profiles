using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Dto.Profile.Receptionist;

public class EditReceptionistProfileDto
{
    public IFormFile? Photo { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public Guid OfficeId { get; set; }
    public bool IsToDeletePhoto { get; set; } = false;
}