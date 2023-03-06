using Innowise.Clinic.Profiles.Services.ConsistencyManager.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.MassTransitService.Consumers;

public class SpecializationUpdatedConsumer : IConsumer<SpecializationUpdatedMessage>
{
    private readonly IConsistencyService _consistencyService;

    public SpecializationUpdatedConsumer(IConsistencyService consistencyService)
    {
        _consistencyService = consistencyService;
    }

    public async Task Consume(ConsumeContext<SpecializationUpdatedMessage> context)
    {
        _consistencyService.EnsureSpecializationConsistency(context.Message);
    }
}