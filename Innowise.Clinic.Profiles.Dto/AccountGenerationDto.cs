using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto;

// TODO MOVE TO A SHARED NUGET PACKAGE
public class AccountGenerationDto
{
    public AccountGenerationDto(Guid entityId, string role, string email)
    {
        EntityId = entityId;
        Role = role;
        Email = email;
    }

    public Guid EntityId { get; }
    public string Role { get; }
    [EmailAddress] public string Email { get; }
}