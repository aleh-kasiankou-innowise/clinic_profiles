using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

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
    public DoctorProfileUpdateDto(IFormFile? Photo, [Required] string FirstName, [Required] string LastName,
        string? MiddleName, DateTime DateOfBirth,
        Guid SpecializationId, Guid OfficeId, DateTime CareerStartYear,
        Guid StatusId, bool isToDeletePhoto = false) : base(StatusId)
    {
        this.Photo = Photo;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.MiddleName = MiddleName;
        this.DateOfBirth = DateOfBirth;
        this.SpecializationId = SpecializationId;
        this.OfficeId = OfficeId;
        this.CareerStartYear = CareerStartYear;
        this.IsToDeletePhoto = isToDeletePhoto;
    }

    [JsonPropertyName("$type")] public new string JsonDiscriminator => "update";
    public IFormFile? Photo { get; init; }
    public bool IsToDeletePhoto { get; set; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? MiddleName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public Guid SpecializationId { get; init; }
    public Guid OfficeId { get; init; }
    public DateTime CareerStartYear { get; init; }

    public void Deconstruct(out IFormFile? photo, [Required] out string firstName, [Required] out string lastName,
        out string? middleName, out DateTime dateOfBirth, out Guid specializationId, out Guid officeId,
        out DateTime careerStartYear, out Guid statusId, bool isToDeletePhoto = false)
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
        isToDeletePhoto = IsToDeletePhoto;
    }
}

public record DoctorProfileDto : DoctorProfileUpdateDto
{
    public DoctorProfileDto(IFormFile? Photo, [Required] string FirstName, [Required] string LastName,
        string? MiddleName, DateTime DateOfBirth,
        [Required] [EmailAddress] string Email, Guid SpecializationId, Guid OfficeId, DateTime CareerStartYear,
        Guid StatusId) : base(Photo, FirstName, LastName, MiddleName, DateOfBirth, SpecializationId,
        OfficeId, CareerStartYear, StatusId)
    {
        this.Email = Email;
    }

    [JsonPropertyName("$type")] public new string JsonDiscriminator => "create";
    public string Email { get; init; }

    public void Deconstruct(out IFormFile? photo, [Required] out string firstName, [Required] out string lastName,
        out string? middleName, out DateTime dateOfBirth,
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