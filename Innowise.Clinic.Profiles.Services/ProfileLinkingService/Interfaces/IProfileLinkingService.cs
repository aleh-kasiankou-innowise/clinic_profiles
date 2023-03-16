using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

namespace Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;

public interface IProfileLinkingService
{
    Task LinkAccountToProfile(UserProfileLinkingRequest profileLinkingDto);
}