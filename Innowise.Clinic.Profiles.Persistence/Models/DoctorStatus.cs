namespace Innowise.Clinic.Profiles.Persistence.Models;

public class DoctorStatus
{
    public Guid StatusId { get; set; }
    public string Name { get; set; }
    public bool IsActiveAccount { get; set; }
    public bool IsAvailableForAppointments { get; set; }
}