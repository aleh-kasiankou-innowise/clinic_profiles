using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters.Abstractions;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters;

public class ReceptionistFilter : ICollectionFilter<Receptionist>
{
    public Expression<Func<Receptionist, bool>> ToFiltrationExpression()
    {
        throw new NotImplementedException();
    }
}