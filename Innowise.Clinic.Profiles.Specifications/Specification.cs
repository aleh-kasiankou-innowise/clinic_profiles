using System.Linq.Expressions;

namespace Innowise.Clinic.Profiles.Specifications;

public class Specification<T>
{
    public readonly Expression<Func<T, bool>> Expression;
    private Func<T, bool> Lambda => LambdaCache<T, bool>.GetLambda(Expression);

    public Specification(Expression<Func<T, bool>> expression)
    {
        Expression = expression;
    }

    public static Specification<T> operator |
        (Specification<T> left, Specification<T> right) =>
        left.Expression.Or(right.Expression);

    public static Specification<T> operator &(Specification<T> left, Specification<T> right) =>
        left.Expression.And(right.Expression);

    public static Specification<T> operator !(Specification<T> specification) =>
        specification.Expression.Not();

    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
    {
        return specification.Expression;
    }

    public static implicit operator Specification<T>(Expression<Func<T, bool>> expression)
    {
        return new Specification<T>(expression);
    }

    public bool IsSatisfiedBy(T entity) => Lambda(entity);
}