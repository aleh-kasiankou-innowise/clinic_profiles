using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Shared.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters.DoctorFilters;

[FilterKey("office")]
public class OfficeFilter : EntityFilter<Doctor>
{
    public override Expression<Func<Doctor, bool>> ToExpression(string value)
    {
        // TODO NOTIFY USER IF GUID IS INCORRECT
        return x => x.OfficeId == Guid.Parse(value);
    }
}