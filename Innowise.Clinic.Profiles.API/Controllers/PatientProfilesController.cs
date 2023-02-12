using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientProfilesController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientProfilesController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [Authorize(Roles = "Receptionist,Doctor,Patient")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ViewPatientProfileDto>> ViewPatientProfile([FromRoute] Guid id)
    {
        return Ok(await _patientService.GetPatientProfileAsync(id));
    }
    
    [Authorize(Roles = "Patient")]
    [HttpPost("my-profile")]
    public async Task<ActionResult<Guid>> CreatePatientProfile([FromBody] CreateEditPatientProfileDto newPatient)
    {
        return Ok((await _patientService.CreateProfileAsync(newPatient)).ToString());
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePatientProfile([FromBody] CreatePatientProfileReceptionistDto newPatient)
    {
        return Ok((await _patientService.CreateProfileAsync(newPatient)).ToString());
    }
    
    [Authorize(Roles = "Patient")]
    [HttpPut("my-profile")]
    public async Task<IActionResult> EditPatientOwnProfile([FromRoute] Guid id, [FromBody] CreateEditPatientProfileDto updatedPatient)
    {
        // extract id from token
        await _patientService.UpdateProfileAsync(id, updatedPatient);
        return Ok();
    }
    
    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditPatientProfile([FromRoute] Guid id, [FromBody] CreateEditPatientProfileDto updatedPatient)
    {
        await _patientService.UpdateProfileAsync(id, updatedPatient);
        return Ok();
    }


    [Authorize(Roles = "Receptionist")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePatientProfile([FromRoute] Guid id)
    {
        await _patientService.DeleteProfileAsync(id);
        return NoContent();
    }
}