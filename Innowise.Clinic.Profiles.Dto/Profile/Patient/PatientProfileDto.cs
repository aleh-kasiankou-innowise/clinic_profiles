using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innowise.Clinic.Profiles.Dto.Profile.Patient;
// TODO USE IFORMFILE INSTEAD OF BYTES
[JsonPolymorphic]
[JsonDerivedType(typeof(PatientProfileDto), "base")]
[JsonDerivedType(typeof(PatientProfileWithNumberAndPhotoDto), "withNumberAndPhoto")]

public record PatientProfileDto([Required] string FirstName, [Required] string LastName,
    string? MiddleName, DateTime DateOfBirth);

public record PatientProfileWithNumberAndPhotoDto : PatientProfileDto
{
    public PatientProfileWithNumberAndPhotoDto(string FirstName, string LastName,
        string? MiddleName, DateTime DateOfBirth, string? PhoneNumber, byte[]? Photo) : base(FirstName, LastName,
        MiddleName, DateOfBirth)
    {
        this.PhoneNumber = PhoneNumber;
        this.Photo = Photo;
    }

    [JsonPropertyName("$type")] public string JsonDiscriminator { get; } = "withNumberAndPhoto";
    public string? PhoneNumber { get; init; }
    public byte[]? Photo { get; init; }

    public void Deconstruct(out string FirstName, out string LastName, out string? MiddleName, out DateTime DateOfBirth, out string? PhoneNumber, out byte[]? Photo)
    {
        FirstName = this.FirstName;
        LastName = this.LastName;
        MiddleName = this.MiddleName;
        DateOfBirth = this.DateOfBirth;
        PhoneNumber = this.PhoneNumber;
        Photo = this.Photo;
    }
}