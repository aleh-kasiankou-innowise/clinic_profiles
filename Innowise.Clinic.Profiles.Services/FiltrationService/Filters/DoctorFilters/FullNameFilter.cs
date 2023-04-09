using System.Linq.Expressions;
using System.Text;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Shared.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Shared.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters.DoctorFilters;

[FilterKey("name")]
public class FullNameFilter : EntityFilter<Doctor>
{
    public override Expression<Func<Doctor, bool>> ToExpression(string value)
    {
        return x => x.Person.FirstName + " " +
            (x.Person.MiddleName == null ? "" : x.Person.MiddleName + " ") +
            x.Person.LastName == value;
    }
}