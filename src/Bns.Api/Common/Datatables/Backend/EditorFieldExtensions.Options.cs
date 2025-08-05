using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;
using Options = DataTables.Options;

namespace Bns.Api.Common.Datatables.Backend;

public static class EditorFieldOptionsExtensions
{
    public static Options DbSetOptions<T>(this DbSet<T> table, Expression<Func<T, object?>> name, Expression<Func<T, object?>> label, Action<Query> WhereQuery = null) where T : Entity
    {
        var options = new Options().Table(table.GetTableNameWithSchema()).Value(table.GetColumnName(name)).Label(table.GetColumnName(label));
        if (WhereQuery is not null) options.Where(WhereQuery);
        return options;
    }

    public static Field DbSetOptions<T>(this Field field, DbSet<T> table, Expression<Func<T, object?>> name, Expression<Func<T, object?>> label, Action<Query>? condition = null, Func<Dictionary<string, object>, string>? format = null) where T : Entity
    {
        return field.Options(table.GetTableNameWithSchema(), table.GetColumnName(name), table.GetColumnName(label), condition, format);
    }
}