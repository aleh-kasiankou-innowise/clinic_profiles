using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientInfoDto>>> GetPatientListing()
    {
        // Possible search by name
        return Ok(await _patientService.GetPatientListingAsync());
    }
}