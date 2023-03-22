using System.ComponentModel.DataAnnotations;
using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Profiles.Specifications;

namespace Innowise.Clinic.Profiles.Persistence.Models;

public class Doctor : IPersonRelatable
{
    public static readonly Specification<Doctor> IsActive = new(x => x.Status.IsActiveAccount);

    public Guid DoctorId { get; set; }
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    public Guid SpecializationId { get; set; }
    public virtual Specialization Specialization { get; set; }
    public Guid OfficeId { get; set; }
    public virtual Office Office { get; set; }
    public DateTime CareerStartYear { get; set; }
    public Guid StatusId { get; set; }
    public virtual DoctorStatus Status { get; set; }
    public DateTime DateOfBirth { get; set; }
}