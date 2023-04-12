using Innowise.Clinic.Profiles.Configuration.Swagger.Examples;
using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Innowise.Clinic.Profiles.Exceptions;
using Innowise.Clinic.Profiles.RequestPipeline;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{UserRoles.Receptionist},{UserRoles.Doctor},{UserRoles.Patient}")]
    [AllowInteractionWithOwnProfileOnlyFilter(UserRoles.Patient)]
    public async Task<ActionResult<ViewPatientProfileDto>> ViewPatientProfile([FromRoute] Guid id)
    {
        return Ok(await _patientService.GetPatientProfileAsync(id));
    }


    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Receptionist},{UserRoles.Patient}")]
    [SwaggerRequestExample(typeof(PatientProfileDto), typeof(CreatePatientProfileExamples))]
    public async Task<ActionResult<Guid>> CreatePatientProfile(
        [FromForm] PatientProfileDto newPatient)
    {
        // TODO ADD LOGIC FOR SEARCHING UNLINKED PROFILES FIRST
        if (User.IsInRole(UserRoles.Receptionist))
            return Ok((await _patientService.CreateProfileAsync(newPatient)).ToString());

        if (newPatient is PatientProfileWithNumberAndPhotoDto newPatientProfile)
        {
            var userId = Guid.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.UserIdClaim).Value);
            var createProfileTask = _patientService.CreateProfileAsync(newPatientProfile, userId);
            return Ok((await createProfileTask).ToString());
        }

        throw new InvalidInputDataException();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{UserRoles.Receptionist},{UserRoles.Patient}")]
    [AllowInteractionWithOwnProfileOnlyFilter(UserRoles.Patient)]
    public async Task<IActionResult> EditPatientProfile([FromRoute] Guid id,
        [FromForm] PatientProfileWithNumberAndPhotoDto updatedPatient)
    {
        await _patientService.UpdateProfileAsync(id, updatedPatient);
        return Ok();
    }


    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoles.Receptionist}")]
    public async Task<IActionResult> DeletePatientProfile([FromRoute] Guid id)
    {
        await _patientService.DeleteProfileAsync(id);
        return NoContent();
    }
}