using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Filters.Abstractions;

public interface ICollectionFilter<T> where T : IPersonRelatable
{
    Expression<Func<T, bool>> ToFiltrationExpression();
}