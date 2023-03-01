using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.RequestPipeline;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class DoctorProfilesController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorProfilesController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [Authorize(Roles = "Receptionist,Doctor")]
    [HttpGet("{id:guid}")]
    [AllowInteractionWithOwnProfileOnlyFilter("Doctor")]
    public async Task<ActionResult<ViewDoctorProfileDto>> ViewProfile([FromRoute] Guid id)
    {
        return Ok(await _doctorService.GetProfileAsync(id));
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProfile([FromBody] DoctorProfileDto newDoctor)
    {
        return Ok((await _doctorService.CreateProfileAsync(newDoctor)).ToString());
    }

// TODO ADD REQUEST EXAMPLES

    [Authorize(Roles = "Receptionist,Doctor")]
    [HttpPut("{id:guid}")]
    [AllowInteractionWithOwnProfileOnlyFilter("Doctor")]
    public async Task<IActionResult> EditProfile([FromRoute] Guid id,
        [FromBody] DoctorProfileStatusDto updatedDoctor)
    {
        if (updatedDoctor is DoctorProfileDto completeUpdateDto)
        {
            await _doctorService.UpdateProfileAsync(id, completeUpdateDto);
        }

        else
        {
            await _doctorService.UpdateStatusAsync(id, updatedDoctor.StatusId);
        }

        return Ok();
    }
}