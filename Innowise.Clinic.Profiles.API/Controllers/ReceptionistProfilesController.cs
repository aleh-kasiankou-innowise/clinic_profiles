using Innowise.Clinic.Profiles.Dto.Profile.Receptionist;
using Innowise.Clinic.Profiles.Services.ReceptionistService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceptionistProfilesController : ControllerBase
{
    private readonly IReceptionistService _receptionistService;

    public ReceptionistProfilesController(IReceptionistService receptionistService)
    {
        _receptionistService = receptionistService;
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Receptionist}")]

    public async Task<ActionResult<Guid>> CreateProfile([FromBody] CreateReceptionistProfileDto newReceptionist)
    {
        return Ok((await _receptionistService.CreateProfileAsync(newReceptionist)).ToString());
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{UserRoles.Receptionist}")]
    public async Task<ActionResult<ViewReceptionistProfileDto>> ViewProfile([FromRoute] Guid id)
    {
        return Ok(await _receptionistService.GetProfileAsync(id));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{UserRoles.Receptionist}")]
    public async Task<IActionResult> EditProfile([FromRoute] Guid id,
        [FromBody] EditReceptionistProfileDto updatedReceptionist)
    {
        await _receptionistService.UpdateProfileAsync(id, updatedReceptionist);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoles.Receptionist}")]
    public async Task<IActionResult> DeleteProfile([FromRoute] Guid id)
    {
        await _receptionistService.DeleteProfileAsync(id);
        return NoContent();
    }
}