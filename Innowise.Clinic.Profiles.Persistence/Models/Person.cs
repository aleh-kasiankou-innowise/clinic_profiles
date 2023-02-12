using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Persistence.Models;

public class Person
{
    public Guid PersonId { get; set; }
    public byte[]? Photo { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    public string? MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Guid UserId { get; set; }
}