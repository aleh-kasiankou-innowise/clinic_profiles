using Innowise.Clinic.Profiles.Persistence.Interfaces;

namespace Innowise.Clinic.Profiles.Persistence.Models;

public class Patient : IPersonRelatable
{
    public Guid PatientId { get; set; }
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}