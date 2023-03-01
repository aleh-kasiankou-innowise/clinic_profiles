namespace Innowise.Clinic.Profiles.Dto.RabbitMq;

[Serializable]
public record AccountStatusChangeDto(Guid AccountId, bool IsActiveStatus);