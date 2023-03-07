using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Services.ReceptionistService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceptionistsController : ControllerBase
{
    private readonly IReceptionistService _receptionistService;

    public ReceptionistsController(IReceptionistService receptionistService)
    {
        _receptionistService = receptionistService;
    }

    [HttpGet]
    [Authorize(Roles = $"{UserRoles.Receptionist}")]
    public async Task<ActionResult<IEnumerable<ReceptionistInfoDto>>> GetListing()
    {
        // add pagination
        return Ok(await _receptionistService.GetListingAsync());
    }
}