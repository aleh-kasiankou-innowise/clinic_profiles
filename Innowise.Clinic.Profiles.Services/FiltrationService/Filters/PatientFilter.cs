using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters.Abstractions;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters;

public class PatientFilter : ICollectionFilter<Patient>
{
    public Expression<Func<Patient, bool>> ToFiltrationExpression()
    {
        throw new NotImplementedException();
    }
}