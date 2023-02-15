using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Services.Constants;
using Innowise.Clinic.Profiles.Services.DoctorService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorInfoDto>>> GetDoctorsListing()
    {
        // TODO ADD FILTERS
        // TODO ADD PAGINATION

        // Possible Filters:
        // Specialization : GUID
        // Office: Guid

        // + search by name

        if (User.IsInRole(UserRoles.Receptionist)) return Ok(await _doctorService.GetListingForReceptionistAsync());

        return Ok(await _doctorService.GetListingAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorInfoDto>> GetDoctorDetails([FromRoute] Guid id)
    {
        return Ok(await _doctorService.GetPublicInfo(id));
    }
}