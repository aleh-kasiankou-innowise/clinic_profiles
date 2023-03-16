namespace Innowise.Clinic.Profiles.Persistence.Models.Interfaces;

public interface IPersonRelatable
{
    public Guid PersonId { get; set; }
    public Person Person { get; set; }
}