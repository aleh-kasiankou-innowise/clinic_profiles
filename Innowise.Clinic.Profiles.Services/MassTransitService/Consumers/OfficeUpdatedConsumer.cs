using Innowise.Clinic.Profiles.Services.ConsistencyManager.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.MassTransitService.Consumers;

public class OfficeUpdatedConsumer : IConsumer<OfficeUpdatedMessage>
{
    private readonly IConsistencyService _consistencyService;

    public OfficeUpdatedConsumer(IConsistencyService consistencyService)
    {
        _consistencyService = consistencyService;
    }

    public async Task Consume(ConsumeContext<OfficeUpdatedMessage> context)
    {
        _consistencyService.EnsureOfficeConsistency(context.Message);
    }
}