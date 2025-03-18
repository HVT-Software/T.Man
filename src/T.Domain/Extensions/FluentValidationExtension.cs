#region

using FluentValidation;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace T.Domain.Extensions;

public static class Regexs
{
    public const string VietnamesePhone = @"^(?:\+84|0)(3[2-9]|5[6|8|9]|7[0|6-9]|8[1-9]|9[0-4|6-9])[0-9]{7}$";
}


public static class FluentValidationExtension
{
    public static IRuleBuilderOptions<T, string> NotEmpty<T, TProperty>(
        this IRuleBuilder<T, string> builder,
        Expression<Func<T, TProperty>> columnSelector)
    {
        string columnName = Description(columnSelector);
        return builder.NotEmpty().WithMessage(o => $"{columnName} không được để trống.");
    }

    public static void MinMax<T>(
        this IRuleBuilder<T, string> builder,
        Expression<Func<T, string>> columnSelector,
        int min,
        int? max = null)
    {
        string columnName = Description(columnSelector);

        if (max != null) { builder.Length(min, max.Value).WithMessage(o => $"{columnName} phải từ {min} đến {max} ký tự."); }
        else { builder.Length(min).WithMessage(o => $"{columnName} phải từ {min} ký tự trở lên."); }
    }

    public static void MinMax<T, TProperty>(
        this IRuleBuilder<T, TProperty> builder,
        Expression<Func<T, TProperty>> columnSelector,
        TProperty min,
        TProperty? max = default) where TProperty : IComparable<TProperty>, IComparable
    {
        string columnName = Description(columnSelector);
        if (max != null) { builder.GreaterThanOrEqualTo(min).WithMessage(o => $"{columnName} phải từ {min} đến {max}."); }
        else { builder.GreaterThan(min).WithMessage(o => $"{columnName} phải từ {min} trở lên."); }
    }

    public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> builder)
    {
        return builder.Matches(Regexs.VietnamesePhone).WithMessage("Số điện thoại không hợp lệ.");
    }

    public static string InValid<T, TProperty>(this T _, Expression<Func<T, TProperty>> columnSelector)
    {
        string columnName = Description(columnSelector);
        return$"[{columnName}] Không đúng định dạng.";
    }

    private static string Description<T, TProperty>(Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new InvalidOperationException("Expression must be a member expression");
        }

        PropertyInfo prop = memberExpression.Member as PropertyInfo
         ?? throw new InvalidOperationException("Expression must be a member expression");

        object[] attrs = prop.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attrs.Length == 0) { return prop.Name; }

        return((DescriptionAttribute)attrs[0]).Description;
    }
}
