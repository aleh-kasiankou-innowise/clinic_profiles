using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Innowise.Clinic.Profiles.Dto.Profile.Doctor;

[JsonPolymorphic]
[JsonDerivedType(typeof(DoctorProfileStatusDto), "base")]
[JsonDerivedType(typeof(DoctorProfileUpdateDto), "update")]
[JsonDerivedType(typeof(DoctorProfileDto), "create")]
public record DoctorProfileStatusDto
{
    public DoctorProfileStatusDto(Guid StatusId)
    {
        this.StatusId = StatusId;
    }

    [JsonPropertyName("$type")] public string JsonDiscriminator => "base";
    public Guid StatusId { get; init; }

    public void Deconstruct(out Guid StatusId)
    {
        StatusId = this.StatusId;
    }
};

public record DoctorProfileUpdateDto : DoctorProfileStatusDto
{
    public DoctorProfileUpdateDto(byte[]? Photo, [Required] string FirstName, [Required] string LastName,
        string MiddleName, DateTime DateOfBirth,
        Guid SpecializationId, Guid OfficeId, DateTime CareerStartYear,
        Guid StatusId) : base(StatusId)
    {
        this.Photo = Photo;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.MiddleName = MiddleName;
        this.DateOfBirth = DateOfBirth;
        this.SpecializationId = SpecializationId;
        this.OfficeId = OfficeId;
        this.CareerStartYear = CareerStartYear;
    }

    [JsonPropertyName("$type")] public new string JsonDiscriminator => "update";
    public byte[]? Photo { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public Guid SpecializationId { get; init; }
    public Guid OfficeId { get; init; }
    public DateTime CareerStartYear { get; init; }

    public void Deconstruct(out byte[]? photo, [Required] out string firstName, [Required] out string lastName,
        out string middleName, out DateTime dateOfBirth, out Guid specializationId, out Guid officeId,
        out DateTime careerStartYear, out Guid statusId)
    {
        photo = Photo;
        firstName = FirstName;
        lastName = LastName;
        middleName = MiddleName;
        dateOfBirth = DateOfBirth;
        specializationId = SpecializationId;
        officeId = OfficeId;
        careerStartYear = CareerStartYear;
        statusId = StatusId;
    }
}

public record DoctorProfileDto : DoctorProfileUpdateDto
{
    public DoctorProfileDto(byte[]? Photo, [Required] string FirstName, [Required] string LastName,
        string MiddleName, DateTime DateOfBirth,
        [Required] [EmailAddress] string Email, Guid SpecializationId, Guid OfficeId, DateTime CareerStartYear,
        Guid StatusId) : base(Photo, FirstName, LastName, MiddleName, DateOfBirth, SpecializationId,
        OfficeId, CareerStartYear, StatusId)
    {
        this.Email = Email;
    }

    [JsonPropertyName("$type")] public new string JsonDiscriminator => "create";
    public string Email { get; init; }

    public void Deconstruct(out byte[]? photo, [Required] out string firstName, [Required] out string lastName,
        out string middleName, out DateTime dateOfBirth,
        [Required] [EmailAddress] out string email, out Guid specializationId, out Guid officeId,
        out DateTime careerStartYear, out Guid statusId)
    {
        photo = Photo;
        firstName = FirstName;
        lastName = LastName;
        middleName = MiddleName;
        dateOfBirth = DateOfBirth;
        email = Email;
        specializationId = SpecializationId;
        officeId = OfficeId;
        careerStartYear = CareerStartYear;
        statusId = StatusId;
    }
    
}