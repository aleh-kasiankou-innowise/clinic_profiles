using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Profiles.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters.PatientFilters;

[FilterKey("name")]
public class FullNameFilter : EntityFilter<Patient>
{
    public override Expression<Func<Patient, bool>> ToExpression()
    {
        return x => x.Person.FirstName + " " +
            (x.Person.MiddleName == null ? "" : x.Person.MiddleName + " ") +
            x.Person.LastName == Value;
    }

    public FullNameFilter(string value) : base(value)
    {
    }
}