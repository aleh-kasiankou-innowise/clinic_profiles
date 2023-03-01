namespace Innowise.Clinic.Profiles.Services.RabbitMqPublisher.Options;

public class RabbitOptions
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string DoctorInactiveRoutingKey { get; set; }
    public string ReceptionistRemovedRoutingKey { get; set; }
    public string ProfilesAuthenticationExchangeName { get; set; }
}