using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Innowise.Clinic.Profiles.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorProfilesController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorProfilesController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [Authorize(Roles = "Doctor")]
    [HttpGet("my-profile")]
    public async Task<ActionResult<ViewDoctorProfileDto>> ViewOwnProfile()
    {
        // extract user id claim and return doctor profile associated with user_id
        var extractedDoctorId = Guid.Empty;
        return Ok(await _doctorService.GetProfileAsync(extractedDoctorId));
    }
    
    [Authorize(Roles = "Receptionist")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ViewDoctorProfileDto>> ViewProfile([FromRoute] Guid id)
    {
        return Ok(await _doctorService.GetProfileAsync(id));
    }
    
    [Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProfile([FromBody] CreateEditDoctorProfileDto newDoctor)
    {
        return Ok((await _doctorService.CreateProfileAsync(newDoctor)).ToString());
    }
    

    [Authorize(Roles = "Doctor")]
    [HttpPut("my-profile")]
    public async Task<IActionResult> EditOwnProfile([FromBody] CreateEditDoctorProfileDto updatedDoctor)
    {
        // extract user id claim and return doctor profile associated with user_id
        var extractedDoctorId = Guid.Empty;
        await _doctorService.UpdateProfileAsync(extractedDoctorId, updatedDoctor);
        return Ok();
    }

    

    [Authorize(Roles = "Receptionist")]
    [HttpPut("status/{id:guid}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] Guid newStatusId)
    {
        await _doctorService.UpdateStatusAsync(id, newStatusId);
        return Ok();
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditProfile([FromRoute] Guid id,
        [FromBody] CreateEditDoctorProfileDto updatedDoctor)
    {
        await _doctorService.UpdateProfileAsync(id, updatedDoctor);
        return Ok();
    }
}