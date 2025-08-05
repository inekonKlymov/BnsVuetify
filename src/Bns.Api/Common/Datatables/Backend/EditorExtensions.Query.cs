using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;

//using System.Data.Entity;
namespace Bns.Api.Common.Datatables.Backend;

public static class EditorQueryExtensions
{
    public static Query DbSetWhere<T>(this Query query, DbSet<T> table, Expression<Func<T, object?>> Key, dynamic value, string op = "=", bool bind = true) where T : Entity
    {
        return query.Where(table.GetColumnName(Key), value, op, bind);
    }

    public static Query DbSetAndWhere<T>(this Query query, DbSet<T> table, Expression<Func<T, object?>> Key, dynamic value, string op = "=", bool bind = true) where T : Entity
    {
        return query.AndWhere($"{table.GetTableNameWithSchema()}.{table.GetColumnName(Key)}", value, op, bind);
    }

    public static Query DbSetAndWhere<T>(this Query query, DbSet<T> table, Expression<Func<T, object?>> Key, IEnumerable<dynamic> values, string op = "=", bool bind = true) where T : Entity
    {
        return query.AndWhere($"{table.GetTableNameWithSchema()}.{table.GetColumnName(Key)}", values, op, bind);
    }

    public static Query DbSetOrWhere<T>(this Query query, DbSet<T> table, Expression<Func<T, object?>> Key, dynamic value, string op = "=", bool bind = true) where T : Entity
    {
        return query.OrWhere($"{table.GetTableNameWithSchema()}.{table.GetColumnName(Key)}", value, op, bind);
    }

    public static Query DbSetOrWhere<T>(this Query query, DbSet<T> table, Expression<Func<T, object?>> Key, IEnumerable<dynamic> values, string op = "=", bool bind = true) where T : Entity
    {
        return query.OrWhere($"{table.GetTableNameWithSchema()}.{table.GetColumnName(Key)}", values, op, bind);
    }
}