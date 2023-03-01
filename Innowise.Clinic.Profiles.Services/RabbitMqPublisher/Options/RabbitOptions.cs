namespace Innowise.Clinic.Profiles.Services.RabbitMqPublisher.Options;

public class RabbitOptions
{
    public string HostName { get; }
    public string UserName { get; }
    public string Password { get; }
    public string DoctorInactiveRoutingKey { get; }
    public string ReceptionistRemovedRoutingKey { get; }
    public string AccountGenerationRoutingKey { get; }
    public string ProfilesAuthenticationExchangeName { get; }
}