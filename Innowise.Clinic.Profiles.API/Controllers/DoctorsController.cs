using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        // Possible Filters:
        // Specialization : GUID
        // Office: Guid

        // + search by name
        // call doctors service?

        // Add pagination
        return Ok(await _doctorService.GetListingAsync());
    }

    [Authorize(Roles = "Receptionist")]
    [HttpGet("reception-grid")]
    public async Task<ActionResult<IEnumerable<DoctorInfoReceptionistDto>>> GetDoctorsListingReceptionist()
    {
        // Possible Filters:
        // Specialization : GUID
        // Office: Guid

        // + search by name
        return Ok(await _doctorService.GetListingForReceptionistAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorInfoDto>> GetDoctorDetails([FromRoute] Guid id)
    {
        return Ok(await _doctorService.GetPublicInfo(id));
    }
}