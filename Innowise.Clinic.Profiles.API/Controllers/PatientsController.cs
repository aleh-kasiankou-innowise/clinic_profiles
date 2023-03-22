using Innowise.Clinic.Profiles.Configuration.Options;
using Innowise.Clinic.Profiles.Dto.Listing;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Profiles.Services.PatientService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Innowise.Clinic.Profiles.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly PaginationConfiguration _paginationConfiguration;

    public PatientsController(IPatientService patientService, IOptions<PaginationConfiguration> paginationConfiguration)
    {
        _patientService = patientService;
        _paginationConfiguration = paginationConfiguration.Value;
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Receptionist},{UserRoles.Doctor}")]
    public async Task<ActionResult<IEnumerable<PatientInfoDto>>> GetPatientListing([FromBody] CompoundFilter<Patient> compoundFilter, [FromQuery] int page)
    {
        var quantity = _paginationConfiguration.PatientsPerPageAdminFrontend;
        return Ok(await _patientService.GetPatientListingAsync(page, quantity, compoundFilter));
    }
}