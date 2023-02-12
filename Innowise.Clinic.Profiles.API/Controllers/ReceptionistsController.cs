using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Services.Interfaces;
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
    public async Task<ActionResult<IEnumerable<ReceptionistInfoDto>>> GetListing()
    {
        // add pagination
        return Ok(await _receptionistService.GetListingAsync());
    }
}