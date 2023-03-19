using Innowise.Clinic.Profiles.Configuration.Options;
using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters;
using Innowise.Clinic.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly PaginationConfiguration _paginationConfiguration;

    public DoctorsController(IDoctorService doctorService, IOptions<PaginationConfiguration> paginationConfiguration)
    {
        _doctorService = doctorService;
        _paginationConfiguration = paginationConfiguration.Value;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorPublicInfoDto>>> GetDoctorsListing([FromQuery] DoctorFilter filter, [FromQuery] int page = 1)
    {
        return Ok(User.IsInRole(UserRoles.Receptionist)
            ? await _doctorService.GetListingForReceptionistAsync(page, _paginationConfiguration.DoctorsPerPageAdminFrontend)
            : await _doctorService.GetListingAsync(page, _paginationConfiguration.DoctorsPerPagePublicFrontend, filter));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorPublicInfoDto>> GetDoctorDetails([FromRoute] Guid id)
    {
        return Ok(await _doctorService.GetPublicInfo(id));
    }
}