using Innowise.Clinic.Profiles.Dto;
using Innowise.Clinic.Profiles.Persistence;
using Innowise.Clinic.Profiles.Services.ProfileLinkingService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
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

    [HttpPost("ensure-created")]
    public async Task<IActionResult> EnsureProfileCreated([FromBody] ProfileConsistencyCheckDto consistencyCheckDto,
        [FromServices] ProfilesDbContext dbContext)
    {
        var isProfileExists = false;

        switch (consistencyCheckDto.Role)
        {
            case "Patient":
                isProfileExists = await dbContext.Patients.AnyAsync(x => x.PersonId == consistencyCheckDto.ProfileId);
                break;
            case "Doctor":
                isProfileExists = await dbContext.Doctors.AnyAsync(x =>
                    x.PersonId == consistencyCheckDto.ProfileId &&
                    x.SpecializationId == consistencyCheckDto.SpecializationId &&
                    x.OfficeId == consistencyCheckDto.OfficeId);
                break;
        }

        return isProfileExists
            ? Ok()
            : BadRequest();
    }
}