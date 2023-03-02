namespace Innowise.Clinic.Profiles.Services.RabbitMq.Options;

public class RabbitOptions
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string DoctorInactiveRoutingKey { get; set; }
    public string ReceptionistRemovedRoutingKey { get; set; }
    public string AccountGenerationRoutingKey { get; set; }
    public string OfficeChangeRoutingKey { get; set; }
    public string SpecializationChangeRoutingKey { get; set; }
    public string OfficesProfilesExchangeName { get; set; }
    public string ProfilesAuthenticationExchangeName { get; set; }
    public string ServicesProfilesExchangeName { get; set; }
}