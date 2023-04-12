using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Dto.Profile.Patient;

[JsonPolymorphic]
[JsonDerivedType(typeof(PatientProfileDto), "base")]
[JsonDerivedType(typeof(PatientProfileWithNumberAndPhotoDto), "withNumberAndPhoto")]
public record PatientProfileDto([Required] string FirstName, [Required] string LastName,
    string? MiddleName, DateTime DateOfBirth);

public record PatientProfileWithNumberAndPhotoDto : PatientProfileDto
{
    public PatientProfileWithNumberAndPhotoDto(string firstName, string lastName,
        string? middleName, DateTime dateOfBirth, string? phoneNumber, IFormFile? photo, bool isToDeletePhoto = false) : base(firstName, lastName,
        middleName, dateOfBirth)
    {
        this.PhoneNumber = phoneNumber;
        this.Photo = photo;
        this.IsToDeletePhoto = isToDeletePhoto;
    }

    [JsonPropertyName("$type")] public string JsonDiscriminator { get; } = "withNumberAndPhoto";
    public string? PhoneNumber { get; init; }
    public IFormFile? Photo { get; init; }
    public bool IsToDeletePhoto { get; set; } = false;

    public void Deconstruct(out string firstName, out string lastName, out string? middleName, out DateTime dateOfBirth,
        out string? phoneNumber, IFormFile? photo, bool isToDeletePhoto = false)
    {
        firstName = this.FirstName;
        lastName = this.LastName;
        middleName = this.MiddleName;
        dateOfBirth = this.DateOfBirth;
        phoneNumber = this.PhoneNumber;
        photo = this.Photo;
        isToDeletePhoto = this.IsToDeletePhoto;
    }
}