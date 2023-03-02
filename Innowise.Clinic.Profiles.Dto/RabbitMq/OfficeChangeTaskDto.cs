namespace Innowise.Clinic.Profiles.Dto.RabbitMq;

// TODO MOVE TO SHARED NUGET PACKAGE
public record OfficeChangeTask(OfficeChange TaskType, OfficeAddressDto OfficeAddress);

public record OfficeAddressDto(Guid OfficeId, string? Address);