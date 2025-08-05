using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper.Internal;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bns.Api.Common.Datatables.Backend;

public static class EditorFieldExtensions
{
    public static Field CreateDataTablesFieldAuto<T>(this DbSet<T> dbSet, Expression<Func<T, object?>> expression, string? tableAliasName = null) where T : Entity
    {
        var tableName = tableAliasName ?? dbSet.GetTableNameWithSchema();
        var columnName = dbSet.GetColumnName(expression);
        var propertyInfo = expression.GetPropertyInfo();
        return new Field($"{tableName}.{columnName}", dbSet.GetAutoFieldName(expression))
            .AddType(propertyInfo)
            .AddAttributes(propertyInfo)
            .AddDefaultFormatters(propertyInfo)
            .AddNullFormatter(dbSet, propertyInfo);
    }

    public static string GetAutoFieldName<T>(this DbSet<T> dbSet, Expression<Func<T, object?>> expression) where T : Entity
    {
        return dbSet.GetColumnNameWithTable(expression);
    }

    public static Field Type<T>(this Field field) => field.Type(typeof(T));

    private static Field AddType(this Field field, PropertyInfo propertyInfo)
    {
        var reqAttr = propertyInfo.PropertyType;
        var strongTypeIdInterface = reqAttr
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrongTypeId<>));

        if (strongTypeIdInterface != null)
        {
            var genericType = strongTypeIdInterface.GetGenericArguments()[0];
            return field.Type(genericType);
        }
        if (reqAttr.IsEnum)
        {
            return field.Type<string>();
        }
        return reqAttr != null
                ? field.Type(reqAttr)
                : field;
    }

    private static Field AddDefaultFormatters(this Field field, PropertyInfo propertyInfo)
    {
        string typeName = "";
        if (propertyInfo.PropertyType.IsNullableType())
        {
            typeName = propertyInfo.PropertyType.GenericTypeArguments.First().Name;
        }
        else
        {
            typeName = propertyInfo.PropertyType.Name;
        }
        return typeName switch
        {
            nameof(DateTime) => field.GetFormatter(GetFormatters.DateTimeFormatter).SetFormatter(SetFormatters.DateTimeFormatter),
            nameof(DateOnly) => field.GetFormatter(GetFormatters.DateOnlyFormatter).SetFormatter(SetFormatters.DateOnlyFormatter),
            nameof(TimeOnly) => field.GetFormatter(GetFormatters.TimeOnlyFormatter).SetFormatter(SetFormatters.TimeOnlyFormatter),
            nameof(DateTimeOffset) => field.Type<DateTime>().GetFormatter(GetFormatters.DateTimeOffset),

            _ => field,
        };
    }

    #region Attributes

    private static Field AddRequiredValidation(this Field field, PropertyInfo propertyInfo)
    {
        var reqAttr = propertyInfo.GetCustomAttribute<RequiredAttribute>();
        return reqAttr != null
            ? field.Validator(Validation.Required(new ValidationOpts() { Message = reqAttr.ErrorMessage }))
            : field;
    }

    private static Field AddMaxLenValidation(this Field field, PropertyInfo propertyInfo)
    {
        var reqAttr = propertyInfo.GetCustomAttribute<MaxLengthAttribute>();
        return reqAttr != null
            ? field.Validator(Validation.MaxLen(reqAttr.Length, new ValidationOpts() { Message = reqAttr.ErrorMessage }))
            : field;
    }

    private static Field AddMinLenValidation(this Field field, PropertyInfo propertyInfo)
    {
        var reqAttr = propertyInfo.GetCustomAttribute<MinLengthAttribute>();
        return reqAttr != null
            ? field.Validator(Validation.MinLen(reqAttr.Length, new ValidationOpts() { Message = reqAttr.ErrorMessage }))
            : field;
    }

    private static Field AddStringLenValidation(this Field field, PropertyInfo propertyInfo)
    {
        var reqAttr = propertyInfo.GetCustomAttribute<StringLengthAttribute>();
        return reqAttr != null
            ? field.Validator(Validation.MinMaxLen(reqAttr.MinimumLength, reqAttr.MaximumLength, new ValidationOpts() { Message = reqAttr.ErrorMessage }))
            : field;
    }

    private static Field AddRegExpLenValidation(this Field field, PropertyInfo propertyInfo)
    {
        var reqAttr = propertyInfo.GetCustomAttribute<RegularExpressionAttribute>();
        return reqAttr != null
            ? field.Validator((val, d, host) => reqAttr.IsValid(val) ? null : reqAttr.ErrorMessage)
            : field;
    }

    private static Field AddAttributes(this Field field, PropertyInfo propertyInfo)
    {
        return field.AddRequiredValidation(propertyInfo)
            .AddMaxLenValidation(propertyInfo)
            .AddMinLenValidation(propertyInfo)
            .AddStringLenValidation(propertyInfo)
            .AddRegExpLenValidation(propertyInfo);

        //.GetFormatter(EditorExtensions.GetFormatters.DateTimeFormatter),
    }

    #endregion Attributes

    private static Field AddNullFormatter<T>(this Field field, DbSet<T> dbSet, PropertyInfo propertyInfo) where T : Entity
    {
        return dbSet.EntityType.FindProperty(propertyInfo.Name).IsNullable ? field.SetFormatter(Format.NullEmpty()) : field;
    }

    public static class GetFormatters
    {
        public static Func<object, Dictionary<string, object>, object?> DebugFormatter => delegate (object val, Dictionary<string, object> data)
        {
            return val;
        };

        public static Func<object, Dictionary<string, object>, object?> FormatStringSpecialSymbols => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            return System.Web.HttpUtility.HtmlDecode(val.ToString());
        };

        public static Func<object, Dictionary<string, object>, object?> DateOnlyFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            string result = "";
            switch (val)
            {
                case DateOnly val1:
                    result = val1.ToDateTime(new TimeOnly(0)).ToString(DateTimeExtensions.DateOnlyConstants.ISO8601SystemFormat);
                    break;

                case DateTime val2:
                    result = val2.ToString(DateTimeExtensions.DateOnlyConstants.ISO8601SystemFormat);
                    break;

                default:
                    return null;
            }
            return result;
        };

        public static Func<object, Dictionary<string, object>, object?> DateTimeFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            string result = "";
            switch (val)
            {
                case DateTime val1:
                    result = val1.ToISO8601();
                    break;

                case TimeSpan val2:
                    var dt = new DateTime(1970, 1, 1).Add(val2);
                    result = dt.ToISO8601();
                    break;

                default:
                    return null;
            }
            return result;
        };

        public static Func<object, Dictionary<string, object>, object?> DateTimeOffset => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            string result = "";
            switch (val)
            {
                case DateTimeOffset val1:
                    result = val1.DateTime.ToISO8601();
                    break;

                default:
                    return null;
            }
            return result;
        };

        public static Func<object, Dictionary<string, object>, object?> TimeOnlyFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            else
            {
                string result = "";
                switch (val)
                {
                    case TimeOnly val1:
                        var ts = val1.ToTimeSpan();
                        result = new DateTime(val1.Ticks).ToString(DateTimeExtensions.TimeOnlyConstants.ISO8601SystemFormat);
                        break;

                    case TimeSpan val2:
                        return val2;
                        var dt = DateTime.Today.Add(val2);
                        result = dt.ToISO8601();
                        break;

                    default:
                        return null;
                }
                return result;
            }
        };

        public static Func<object, Dictionary<string, object>, object?> BitFormatter => delegate (object val, Dictionary<string, object> data)
        {
            var tryparse = bool.TryParse(val.ToString(), out bool boolVal);
            if (!tryparse) return true;
            if (tryparse && boolVal == false) return false;
            return null;
        };

        public static Func<object, Dictionary<string, object>, object?> EnumFormatter<T>() where T : Enum => delegate (object val, Dictionary<string, object> data)
        {
            return (T)Enum.Parse(typeof(T), val.ToString());
        };

        public static Func<object, Dictionary<string, object>, object?> XmlFormatter<T>() where T : class, new() => delegate (object val, Dictionary<string, object> data)
        {
            if (val is null || string.IsNullOrEmpty(val.ToString())) return new T();
            return SerializationHelper.Deserialize<T>(val.ToString());
        };

        public static Func<object, Dictionary<string, object>, object?> JsonFormatter<T>() where T : class, new() => delegate (object val, Dictionary<string, object> data)
        {
            if (val is null || string.IsNullOrEmpty(val.ToString())) return new T();
            return JsonConvert.DeserializeObject<T>(val.ToString());
        };
    }

    public static class SetFormatters
    {
        public static Func<object, Dictionary<string, object>, object?> EmptyToNullFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            return val;
        };

        public static Func<object, Dictionary<string, object>, object?> DebugFormatter => delegate (object val, Dictionary<string, object> data)
        {
            return val;
        };

        public static Func<object, Dictionary<string, object>, object?> FormatStringSpecialSymbols => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            return System.Web.HttpUtility.HtmlDecode(val.ToString());
        };

        public static Func<object, Dictionary<string, object>, object?> DateOnlyFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            return val;
            if (DateTime.TryParse(Convert.ToString(val), out DateTime datetime))
            {
                return datetime.ToISO8601();
            }
            return null;
        };

        public static Func<object, Dictionary<string, object>, object?> DateTimeFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value)
            {
                return null;
            }
            return val;
            if (DateTime.TryParse(Convert.ToString(val), out DateTime datetime))
            {
                return datetime.ToISO8601();
            }
            return null;
        };

        public static Func<object, Dictionary<string, object>, object?> TimeOnlyFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value) return null;
            return val;
            if (DateTime.TryParse(Convert.ToString(val), out DateTime timeonly))
            {
                return timeonly.TimeOfDay.ToString(@"hh\:mm\:ss");
                //.TimeOfDay.ToISO8601();
            }
            return null;
        };

        //public static Func<object, Dictionary<string,object>,object?> DateTimeOffset => delegate(object val, Dictionary<string,object> data)
        //{
        //    if (val == null || val as string == "" || val == DBNull.Value)
        //    {
        //        return null;
        //    }
        //    if (DateTime.TryParse(Convert.ToString(val), out DateTime datetime))
        //        datetime = DateTime.SpecifyKind(datetime, DateTimeKind.Local);

        //        DateTimeOffset localTime2 = datetime;
        //    return localTime2.ToIso
        //    }
        //    return null;
        //};
        public static Func<object, Dictionary<string, object>, object?> BitFormatter => delegate (object val, Dictionary<string, object> data)
        {
            if (val == null || val as string == "" || val == DBNull.Value) return null;
            return TimeOnly.TryParse(Convert.ToString(val), out TimeOnly timeonly) ? timeonly : null;
        };

        public static Func<object, Dictionary<string, object>, object?> XmlFormatter<T>(Func<object, Dictionary<string, object>, bool> condition = null) where T : class, new() => delegate (object val, Dictionary<string, object> data)
        {
            if (condition is not null && !condition(val, data)) return null;
            Dictionary<string, object> val1 = val as Dictionary<string, object>;
            T config = new();
            foreach (var pi in config.GetType().GetProperties())
            {
                if (val1.TryGetValue(pi.Name, out object? resOut))
                {
                    var converter = TypeDescriptor.GetConverter(pi.PropertyType);
                    pi.SetValue(config, converter.ConvertFrom(resOut.ToString()));
                }
            }

            return SerializationHelper.Serialize(config);
        };

        public static Func<object, Dictionary<string, object>, object?> JsonFormatter<T>() where T : class, new() => delegate (object val, Dictionary<string, object> data)
        {
            Dictionary<string, object> val1 = val as Dictionary<string, object>;
            T config = new();
            foreach (var pi in config.GetType().GetProperties())
            {
                if (val1.TryGetValue(pi.Name, out object? resOut))
                {
                    var converter = TypeDescriptor.GetConverter(pi.PropertyType);
                    pi.SetValue(config, converter.ConvertFrom(resOut.ToString()));
                }
            }

            return JsonConvert.SerializeObject(config);
        };
    }


    public static Field Field<T>(this Editor editor, DbSet<T> set, Expression<Func<T, object?>> expression) where T : Entity => editor.Field(set.GetAutoFieldName(expression));
}