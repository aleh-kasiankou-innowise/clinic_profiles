using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;
using Innowise.Clinic.Profiles.Services.FiltrationService.Attributes;
using Innowise.Clinic.Profiles.Specifications;
using MassTransit.Internals;

namespace Innowise.Clinic.Profiles.Services.FiltrationService;

public class FilterResolver<T> where T : IPersonRelatable
{
    private ConcurrentDictionary<string, Type> FilterRegistry { get; } = new();


    public FilterResolver()
    {
        var filterAssembly = Assembly.GetAssembly(typeof(IEntityFilter<>)) ??
                             throw new ApplicationException("The assembly with filters does not exist.");

        var filterTypes = filterAssembly
            .GetTypes()
            .Where(x => x.IsSubclassOf(typeof(EntityFilter<T>)) && !x.HasAttribute<DisabledFilterAttribute>());

        foreach (var filter in filterTypes)
        {
            Console.WriteLine($"Registering new filter: {filter.FullName}");
            var filterKey = filter.GetCustomAttribute<FilterKeyAttribute>()?.FilterKey ??
                            throw new ApplicationException($"The filter must have a filter key: {filter.FullName}");

            if (!FilterRegistry.TryAdd(filterKey, filter))
            {
                throw new ApplicationException(
                    $"The filter keys must be unique. " +
                    $"The {filterKey} is already reserved by class {FilterRegistry[filterKey].FullName}");
            }
        }
    }

    public Func<string, Expression<Func<T, bool>>> BuildExpression(Type filterType)
    {
        // TODO THIS IS SLOW. NEED IMPROVEMENT
        // ideas:
        // use FastCompile
        // create expressions cache
        // create response cache
        
        var filterValueParam = Expression.Parameter(typeof(string), "filterValue");
        var filterInstance = Expression.New(filterType.GetConstructor(new[] { typeof(string) }), filterValueParam);
        var lambda = Expression.Lambda<Func<string, Expression<Func<T, bool>>>>(
            Expression.Call(filterInstance, "ToExpression", Type.EmptyTypes), new[] { filterValueParam }
        );
        return lambda.Compile();
    }

    public Expression<Func<T, bool>> ConvertCompoundFilterToExpression(ICompoundFilter<T> compoundFilter)
    {
        var filtrationExpressions = new List<Expression<Func<T, bool>>>();
        foreach (var filter in compoundFilter.Filters)
        {
            if (FilterRegistry.TryGetValue(filter.Key, out var filterType))
            {
                var expression = BuildExpression(filterType)(filter.Value);
                Console.WriteLine(expression);
                filtrationExpressions.Add(expression);
            }
            else
            {
                Console.WriteLine($"The filter {filter.Key} is not available.");
                foreach (var availableFilter in FilterRegistry)
                {
                    Console.WriteLine("Available filters:");
                    Console.WriteLine(availableFilter.Key);
                }
            }
        }

        if (!filtrationExpressions.Any())
        {
            return x => true;
        }
        
        var filtrationExpression = filtrationExpressions.Count > 1
            ? filtrationExpressions.Aggregate((first, second) => first
                .And(second))
            : filtrationExpressions[0];
        return filtrationExpression;
    }
}