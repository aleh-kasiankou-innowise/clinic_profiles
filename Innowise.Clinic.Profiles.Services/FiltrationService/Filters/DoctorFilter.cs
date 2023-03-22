using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters.Abstractions;
using Innowise.Clinic.Profiles.Specifications;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters;

public class DoctorFilter : ICollectionFilter<Doctor>
{
    public string? FullName { get; set; } = null;
    public Guid? SpecializationId { get; set; } = null;
    public Guid? OfficeId { get; set; } = null;

    public Expression<Func<Doctor, bool>> ToFiltrationExpression()
    {
        var filtrationExpressions = new List<Expression<Func<Doctor, bool>>>();

        // TODO IMPLEMENT OPEN CLOSED PRINCIPLE (create a separate class for filter key-value pairs and add expressions there)

        if (FullName is not null)
        {
            filtrationExpressions.Add(x => x.Person.FirstName + " " +
                (x.Person.MiddleName == null ? "" : x.Person.MiddleName + " ") +
                x.Person.LastName == FullName);
        }

        if (SpecializationId is not null)
        {
            filtrationExpressions.Add(x => x.SpecializationId == SpecializationId);
        }

        if (OfficeId is not null)
        {
            filtrationExpressions.Add(x => x.OfficeId == OfficeId);
        }

        var filtrationExpression = filtrationExpressions.Aggregate((first, second) => first.And(second));
        return filtrationExpression;
    }
}