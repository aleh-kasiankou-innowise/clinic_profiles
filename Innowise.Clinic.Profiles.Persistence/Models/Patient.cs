using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Profiles.Specifications;

namespace Innowise.Clinic.Profiles.Persistence.Models;

public class Patient : IPersonRelatable
{
    public static readonly Specification<Patient> IsLinkedToAccount = new(x => x.Person.UserId != null);

    public Guid PatientId { get; set; }
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}