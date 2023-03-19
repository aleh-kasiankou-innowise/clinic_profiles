using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters.Abstractions;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters;

public class DoctorFilter : ICollectionFilter<Doctor>
{
    public string? FullName { get; set; } = null;
    public Guid? SpecializationId { get; set; } = null;
    public Guid? OfficeId { get; set; } = null;
    
    public Expression<Func<Doctor, bool>> ToFiltrationExpression()
    {
        var filtrationExpressions = new List<Expression<Func<Doctor, bool>>>();

        if (FullName is not null)
        {
            filtrationExpressions.Add(x =>
                x.Person.FirstName + " " + (x.Person.MiddleName == null ? "" : x.Person.MiddleName + " ") +
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

        Expression<Func<Doctor, bool>> filtrationExpression = null;
        var doctorParameter = Expression.Parameter(typeof(Doctor), "x");

        // TODO REPLACE INVOKE WITH MORE SUPPORTED METHODS
        for (int i = 0; i < filtrationExpressions.Count; i++)
        {
            filtrationExpression =
                i > 0
                    ? Expression.Lambda<Func<Doctor, bool>>(
                        Expression.AndAlso(Expression.Invoke(filtrationExpression, doctorParameter),
                            Expression.Invoke(filtrationExpressions[i], doctorParameter)), doctorParameter)
                    : filtrationExpressions[i];
        }

        return filtrationExpression ?? (x => true);
    }
}

