namespace Innowise.Clinic.Profiles.Dto.RabbitMq;

// TODO MOVE TO SHARED NUGET PACKAGE
public record SpecializationChangeTaskDto(SpecializationChange TaskType, SpecializationDto SpecializationDto);

public record SpecializationDto(Guid SpecializationId, string SpecializationName);