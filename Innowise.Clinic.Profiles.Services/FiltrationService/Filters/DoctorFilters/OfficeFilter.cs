using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Profiles.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters.DoctorFilters;

[FilterKey("office")]
public class OfficeFilter : EntityFilter<Doctor>
{
    public OfficeFilter(string value) : base(value)
    {
    }

    public override Expression<Func<Doctor, bool>> ToExpression()
    {
        // TODO NOTIFY USER IF GUID IS INCORRECT
        return x => x.OfficeId == Guid.Parse(Value);
    }
}