using Innowise.Clinic.Profiles.Dto;

namespace Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;

public interface IProfileLinkingService
{
    Task LinkAccountToProfile(UserProfileLinkingDto profileLinkingDto);
}