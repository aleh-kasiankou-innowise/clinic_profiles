using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.RequestPipeline;
using Innowise.Clinic.Profiles.Services.Constants;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
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
    [AllowInteractionWithOwnProfileOnlyFilter("Patient")]
    public async Task<ActionResult<ViewPatientProfileDto>> ViewPatientProfile([FromRoute] Guid id)
    {
        return Ok(await _patientService.GetPatientProfileAsync(id));
    }


    // TODO ADD SWAGGER SAMPLE REQUESTS FOR EACH TYPE 
    [Authorize(Roles = "Receptionist,Patient")]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePatientProfile(
        [FromBody] PatientProfileDto newPatient)
    {
        // TODO ADD LOGIC FOR SEARCHING UNLINKED PROFILES FIRST

        if (User.IsInRole(UserRoles.Receptionist))
            return Ok((await _patientService.CreateProfileAsync(newPatient)).ToString());


        if (newPatient is PatientProfileWithNumberAndPhotoDto newPatientProfile)
        {
            var userId = User.Claims.First(x => x.Type == JwtClaimTypes.UserIdClaim).Value;
            return Ok((await _patientService.CreateProfileAsync(newPatientProfile, Guid.Parse(userId)))
                .ToString());
        }


        throw new InvalidOperationException("The user has sent the base class");
    }

    [Authorize(Roles = "Receptionist, Patient")]
    [HttpPut("{id:guid}")]
    [AllowInteractionWithOwnProfileOnlyFilter("Patient")]
    public async Task<IActionResult> EditPatientProfile([FromRoute] Guid id,
        [FromBody] PatientProfileWithNumberAndPhotoDto updatedPatient)
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