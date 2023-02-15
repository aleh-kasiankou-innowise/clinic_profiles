using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("{controller}")]
public class HelperServicesController : ControllerBase
{
    private readonly IProfileLinkingService _profileLinkingService;

    public HelperServicesController(IProfileLinkingService profileLinkingService)
    {
        _profileLinkingService = profileLinkingService;
    }

    [HttpPost("link-account")]
    public async Task<IActionResult> LinkAccountToProfile([FromBody] UserProfileLinkingDto profileLinkingDto)
    {
        await _profileLinkingService.LinkAccountToProfile(profileLinkingDto);
        return Ok();
    }
}