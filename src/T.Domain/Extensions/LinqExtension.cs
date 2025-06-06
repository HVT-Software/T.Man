﻿#region

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

#endregion

namespace T.Domain.Extensions;

public static class LinqExtension {
    public static IQueryable<TSource> WhereDate<TSource>(
        this IQueryable<TSource> source,
        DateTimeOffset? from,
        DateTimeOffset? to,
        Expression<Func<TSource, DateTimeOffset>> selector) {
        ParameterExpression parameter = selector.Parameters[0];

        if (from.HasValue) {
            BinaryExpression body       = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(from.Value));
            var              expression = Expression.Lambda<Func<TSource, bool>>(body, parameter);
            source = source.Where(expression);
        }

        if (to.HasValue) {
            BinaryExpression body       = Expression.LessThanOrEqual(selector.Body, Expression.Constant(to.Value));
            var              expression = Expression.Lambda<Func<TSource, bool>>(body, parameter);
            source = source.Where(expression);
        }

        return source;
    }

    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        bool when,
        Expression<Func<TSource, bool>> predicateTrue,
        Expression<Func<TSource, bool>>? predicateFalse = null) {
        if (when) { return source.Where(predicateTrue); }

        return predicateFalse != null ? source.Where(predicateFalse) : source;
    }

    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> when,
        Expression<Func<TSource, bool>> predicateTrue) {
        var expression = Expression.Lambda<Func<TSource, bool>>(Expression.Or(Expression.And(when, predicateTrue), Expression.Not(when)));

        return source.Where(expression);
    }

    public static IQueryable<TSource> WhereFunc<TSource>(
        this IQueryable<TSource> source,
        bool when,
        Func<IQueryable<TSource>, IQueryable<TSource>> funcTrue,
        Func<IQueryable<TSource>, IQueryable<TSource>>? funcFalse = null) {
        if (when) { return funcTrue.Invoke(source); }

        return funcFalse != null ? funcFalse.Invoke(source) : source;
    }

    public static async Task<int> CountIf<TSource, TResult>(
        this IQueryable<TSource> source,
        bool when,
        Expression<Func<TSource, TResult>>? selector = null,
        CancellationToken cancellationToken = default) {
        if (!when) { return 0; }

        if (selector != null) { return await source.Select(selector).CountAsync(cancellationToken); }

        return await source.CountAsync(cancellationToken);
    }

    public static IQueryable<TSource> Paging<TSource>(
        this IQueryable<TSource> source,
        int skip,
        int take) {
        return source.Paging(true, skip, take);
    }

    public static IQueryable<TSource> Paging<TSource>(
        this IQueryable<TSource> source,
        bool when,
        int skip,
        int take) {
        return when ? source.Skip(skip).Take(take) : source;
    }
}
