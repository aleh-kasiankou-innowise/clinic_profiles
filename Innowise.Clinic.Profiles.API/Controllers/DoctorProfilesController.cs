using Innowise.Clinic.Profiles.AppConfiguration.Swagger.Examples;
using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.RequestPipeline;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Receptionist,Doctor")]
    [AllowInteractionWithOwnProfileOnlyFilter("Doctor")]
    public async Task<ActionResult<ViewDoctorProfileDto>> ViewProfile([FromRoute] Guid id)
    {
        return Ok(await _doctorService.GetProfileAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "Receptionist")]
    public async Task<ActionResult<Guid>> CreateProfile([FromBody] DoctorProfileDto newDoctor)
    {
        return Ok((await _doctorService.CreateProfileAsync(newDoctor)).ToString());
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Receptionist,Doctor")]
    [AllowInteractionWithOwnProfileOnlyFilter("Doctor")]
    [SwaggerRequestExample(typeof(DoctorProfileStatusDto), typeof(UpdateDoctorProfileInfoExamples))]
    public async Task<IActionResult> EditProfile([FromRoute] Guid id,
        [FromBody] DoctorProfileStatusDto updatedDoctor)
    {
        if (updatedDoctor is DoctorProfileDto completeUpdateDto)
        {
            await _doctorService.UpdateProfileAsync(id, completeUpdateDto);
            return Ok();
        }

        if (updatedDoctor.GetType() == typeof(DoctorProfileStatusDto))
        {
            await _doctorService.UpdateStatusAsync(id, updatedDoctor.StatusId);
            return Ok();
        }

        throw new InvalidInputDataException();
    }
}