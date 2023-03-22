using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Profiles.Services.FiltrationService.Attributes;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;

[FilterKey("base")]
public abstract class EntityFilter<T> : IEntityFilter<T> where T : IPersonRelatable
{
    protected EntityFilter(string value)
    {
        Value = value;
    }

    public string Value { get; }
    public abstract Expression<Func<T, bool>> ToExpression();
}