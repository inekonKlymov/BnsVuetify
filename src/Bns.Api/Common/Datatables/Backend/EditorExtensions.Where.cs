using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;

//using System.Data.Entity;
namespace Bns.Api.Common.Datatables.Backend;

public static class EditorWhereExtensions
{
    public static Editor WhereDbSet<T>(this Editor editor, DbSet<T> dbSet, Expression<Func<T, object?>> expression, object value, string op = "=", string? tableAlias = null) where T : Entity
    {
        var tableName = tableAlias ?? dbSet.GetTableNameWithSchema();
        var columnName = dbSet.GetColumnName(expression);
        return editor.Where($"{tableName}.{columnName}", value, op);
    }

    public static Editor WhereDbSet<T>(this Editor editor, DbSet<T> dbSet, Expression<Func<T, object?>> expression, Action<Query> fn, object value, string op = "=") where T : Entity
    {
        var tableName = dbSet.GetTableNameWithSchema();
        var columnName = dbSet.GetColumnName(expression);
        return editor.Where(fn);
    }
}