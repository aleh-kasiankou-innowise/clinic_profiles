using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innowise.Clinic.Profiles.Dto.Profile.Patient;

[JsonPolymorphic]
[JsonDerivedType(typeof(PatientProfileWithNumberAndPhotoDto), "withNumberAndPhoto")]
public record PatientProfileDto(DateTime DateOfBirth, [Required] string FirstName, [Required] string LastName,
    string? MiddleName);

public record PatientProfileWithNumberAndPhotoDto(DateTime DateOfBirth, string FirstName, string LastName,
    string? MiddleName, string? PhoneNumber, byte[]? Photo) : PatientProfileDto(DateOfBirth, FirstName, LastName,
    MiddleName);