using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Dto.Profile.Doctor;

public record DoctorProfileStatusDto(Guid StatusId);

public record DoctorProfileDto(byte[]? Photo, [Required] string FirstName, [Required] string LastName,
    string MiddleName, DateTime DateOfBirth,
    [Required] [EmailAddress] string Email, Guid SpecializationId, Guid OfficeId, DateTime CareerStartYear,
    Guid StatusId) : DoctorProfileStatusDto(StatusId);