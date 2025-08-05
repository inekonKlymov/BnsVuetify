using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

//using System.Data.Entity;
namespace Bns.Api.Common;

public static class ProperyInfoExtensions
{
    public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object?>> expression)
    {
        MemberExpression? memberExpression = null;
        memberExpression = expression.Body.NodeType switch
        {
            ExpressionType.Convert or ExpressionType.ConvertChecked => (expression.Body is UnaryExpression ue ? ue.Operand : null) as MemberExpression,
            _ => expression.Body as MemberExpression,
        };
        return (PropertyInfo)memberExpression!.Member;
    }

    public static PropertyInfo GetPropertyInfo<T, B>(this Expression<Func<T, B?>> expression)
    {
        MemberExpression? memberExpression = null;
        memberExpression = expression.Body.NodeType switch
        {
            ExpressionType.Convert or ExpressionType.ConvertChecked => (expression.Body is UnaryExpression ue ? ue.Operand : null) as MemberExpression,
            _ => expression.Body as MemberExpression,
        };
        return (PropertyInfo)memberExpression!.Member;
    }

    public static Tattr? GetCustomAttribute<Tattr, T>(this Expression<Func<T, object?>> expression) where Tattr : Attribute
    {
        return expression.GetPropertyInfo().GetCustomAttribute<Tattr>();
    }

    public static string GetEnumMemberValue<TModel>(this TModel enumValue) where TModel : Enum
    {
        EnumMemberAttribute? displayAttribute = enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<EnumMemberAttribute>();
        if (displayAttribute is null)
            return "";

        return displayAttribute.Value ?? "";
    }
}