using Innowise.Clinic.Profiles.Configuration.Options;
using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Services.ReceptionistService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceptionistsController : ControllerBase
{
    private readonly IReceptionistService _receptionistService;
    private readonly PaginationConfiguration _paginationConfiguration;


    public ReceptionistsController(IReceptionistService receptionistService,
        IOptions<PaginationConfiguration> paginationConfiguration)
    {
        _receptionistService = receptionistService;
        _paginationConfiguration = paginationConfiguration.Value;
    }

    [HttpGet]
    [Authorize(Roles = $"{UserRoles.Receptionist}")]
    public async Task<ActionResult<IEnumerable<ReceptionistInfoDto>>> GetListing([FromQuery] int page = 1)
    {
        var quantity = _paginationConfiguration.ReceptionistsPerPageAdminFrontend;
        return Ok(await _receptionistService.GetListingAsync(page, quantity));
    }
}