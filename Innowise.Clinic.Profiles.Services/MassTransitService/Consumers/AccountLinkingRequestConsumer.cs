using Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;

namespace Innowise.Clinic.Profiles.Services.MassTransitService.Consumers;

public class AccountLinkingRequestConsumer : IConsumer<UserProfileLinkingRequest>
{
    private readonly IProfileLinkingService _profileLinkingService;

    public AccountLinkingRequestConsumer(IProfileLinkingService profileLinkingService)
    {
        _profileLinkingService = profileLinkingService;
    }

    public async Task Consume(ConsumeContext<UserProfileLinkingRequest> context)
    {
        await _profileLinkingService.LinkAccountToProfile(context.Message);
        await context.RespondAsync<UserProfileLinkingResponse>(new(true));
    }
}