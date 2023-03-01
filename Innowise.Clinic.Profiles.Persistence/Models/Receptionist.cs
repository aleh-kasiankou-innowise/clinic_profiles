using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Profiles.Persistence.Models;

public class Receptionist
{
    public Guid ReceptionistId { get; set; }
    public Guid PersonId { get; set; }
    public virtual Person Person { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    public Guid OfficeId { get; set; }
    public virtual Office Office { get; set; }
}