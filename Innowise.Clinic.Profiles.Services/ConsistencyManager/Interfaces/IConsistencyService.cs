using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

namespace Innowise.Clinic.Profiles.Services.ConsistencyManager.Interfaces;

public interface IConsistencyService
{
    void EnsureOfficeConsistency(OfficeUpdatedMessage officeChangeTask);
    void EnsureSpecializationConsistency(SpecializationUpdatedMessage specializationChangeTask);
}