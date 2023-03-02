using Innowise.Clinic.Profiles.Dto.RabbitMq;

namespace Innowise.Clinic.Profiles.Services.ConsistencyManager.Interfaces;

public interface IConsistencyService
{
    void EnsureOfficeConsistency(OfficeChangeTask officeChangeTask);
    void EnsureSpecializationConsistency(SpecializationChangeTaskDto specializationChangeTask);
}