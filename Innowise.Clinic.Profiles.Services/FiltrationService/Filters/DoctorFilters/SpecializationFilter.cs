using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Profiles.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters.DoctorFilters;

[FilterKey("specialization")]
public class SpecializationFilter : EntityFilter<Doctor>
{
    public SpecializationFilter(string value) : base(value)
    {
    }

    public override Expression<Func<Doctor, bool>> ToExpression()
    {
        // TODO NOTIFY USER IF GUID IS INCORRECT
        return x => x.SpecializationId == Guid.Parse(Value);
    }
}