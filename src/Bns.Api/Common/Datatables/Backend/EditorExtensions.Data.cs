using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using Microsoft.EntityFrameworkCore;

//using System.Data.Entity;
namespace Bns.Api.Common.Datatables.Backend;

public static class EditorDataExtensions
{
    public static object? ExtractFromData<T>(this DbSet<T> table, Dictionary<string, object>? data, Expression<Func<T, object?>> name) where T : Entity
    {
        var objectName = table.GetTableName();
        var obj = data[objectName] as Dictionary<string, object>;

        if (obj is null) return null;
        var colName = table.GetColumnName(name);
        return obj[colName];
    }

    public static object? ExtractFromData<TModel>(Dictionary<string, object> data, DbContext dbSet, Expression<Func<TModel, object?>> valueFunc) where TModel : Entity
    {
        return dbSet.Set<TModel>().ExtractFromData(data, valueFunc);
    }

    public static object? ExtractFromData<TModel>(Dictionary<string, object> data, DbSet<TModel> table, Expression<Func<TModel, object?>> valueFunc) where TModel : Entity
    {
        return table.ExtractFromData(data, valueFunc);
    }

    public static string? ExtractFromData(Dictionary<string, object> data, string name)
    {
        var dotIdx = name.IndexOf('.');
        if (dotIdx == -1)
        {
            return data.TryGetValue(name, out object? value) ? value.ToString() : null;
        }
        var first = name.Substring(0, dotIdx);
        var second = name.Substring(dotIdx + 1);
        var subdata = data[first] as Dictionary<string, object>;
        if (subdata is null) return null;
        return ExtractFromData(subdata, second);
    }

    public static T? ExtractFromData<T>(Dictionary<string, object> data, string name)
    {
        var dotIdx = name.IndexOf('.');
        if (dotIdx == -1)
        {
            if (data.TryGetValue(name, out object? value))
            {
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Boolean:
                        var val = value.ToString();
                        object res;
                        if (val == "1" || val == "0")
                        {
                            res = val == "1";
                        }
                        else
                        {
                            res = val;
                        }
                        return (T)Convert.ChangeType(res, typeof(T));

                    default:
                        return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            else
            {
                return default;
            }
        }
        var first = name.Substring(0, dotIdx);
        var second = name.Substring(dotIdx + 1);
        var subdata = data[first] as Dictionary<string, object>;
        if (subdata is null) return default;
        return ExtractFromData<T>(subdata, second);
    }

    public static T? ExtractFromData<T, TModel>(Dictionary<string, object> data, DbContext dbSet, Expression<Func<TModel, object?>> valueFunc) where TModel : Entity
    {
        var name = dbSet.Set<TModel>().GetColumnNameWithTable(valueFunc);
        return ExtractFromData<T>(data, name);
    }

    public static void WriteToDatatablesData<T>(this DbSet<T> table, Dictionary<string, object>? data, Expression<Func<T, object?>> name, object value) where T : Entity
    {
        var objectName = table.GetTableName();
        var obj = data[objectName] as Dictionary<string, object>;
        if (obj is null) return;
        var colName = table.GetColumnName(name);
        obj[colName] = value;
    }
}