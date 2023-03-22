using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;

public interface IEntityFilter<T> where T: IPersonRelatable
{
    public string Value { get; }
    public Expression<Func<T, bool>> ToExpression();
}