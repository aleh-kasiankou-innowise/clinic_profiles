using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Innowise.Clinic.Profiles.Specifications;

public class LambdaCache<TIn, TOut>
{
    private static readonly ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>> Cache = new();

    public static Func<TIn, TOut> GetLambda(Expression<Func<TIn, TOut>> expression) =>
        Cache.GetOrAdd(expression, x => x.Compile());
}