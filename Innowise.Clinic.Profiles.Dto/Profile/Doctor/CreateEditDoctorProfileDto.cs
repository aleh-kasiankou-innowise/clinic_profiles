using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Profiles.Dto.Profile.Doctor;

public abstract record PolymorphicDoctorProfileBase(string ModelType);
public record DoctorProfileStatusDto(Guid StatusId) : PolymorphicDoctorProfileBase("status_update");

public record DoctorProfileUpdateDto(IFormFile? Photo, [Required] string FirstName, [Required] string LastName,
    string? MiddleName, DateTime DateOfBirth,
    Guid SpecializationId, Guid OfficeId, DateTime CareerStartYear,
    Guid StatusId, bool IsToDeletePhoto = false) : PolymorphicDoctorProfileBase("profile_update");

public record DoctorProfileDto(IFormFile? Photo, [Required] string FirstName, [Required] string LastName,
    string? MiddleName, DateTime DateOfBirth,
    [Required] [EmailAddress] string Email, Guid SpecializationId, Guid OfficeId, DateTime CareerStartYear,
    Guid StatusId) : PolymorphicDoctorProfileBase("profile_create");