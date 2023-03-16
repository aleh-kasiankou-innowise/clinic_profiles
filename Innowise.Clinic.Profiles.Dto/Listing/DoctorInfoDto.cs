using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innowise.Clinic.Profiles.Dto.Listing;

[JsonPolymorphic]
[JsonDerivedType(typeof(DoctorPublicInfoDto), "base")]
[JsonDerivedType(typeof(DoctorInfoReceptionistDto), "receptionist")]
public record DoctorInfoBaseDto(Guid DoctorId, string FirstName, string LastName, string? MiddleName,
    Guid SpecializationId, Guid OfficeId);

public record DoctorPublicInfoDto(Guid DoctorId, string FirstName, string LastName, string? MiddleName,
    Guid SpecializationId, Guid OfficeId, [Required] DateTime CareerStartYear, byte[]? Photo) : DoctorInfoBaseDto(
    DoctorId,
    FirstName, LastName, MiddleName,
    SpecializationId, OfficeId);

public record DoctorInfoReceptionistDto(Guid DoctorId, string FirstName, string LastName, string? MiddleName,
    Guid SpecializationId, Guid OfficeId, [Required] Guid StatusId) : DoctorInfoBaseDto(DoctorId, FirstName, LastName,
    MiddleName,
    SpecializationId, OfficeId);