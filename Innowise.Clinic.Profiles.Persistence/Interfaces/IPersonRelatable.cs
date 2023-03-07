using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Persistence.Interfaces;

public interface IPersonRelatable
{
    public Guid PersonId { get; set; }
    public Person Person { get; set; }
}