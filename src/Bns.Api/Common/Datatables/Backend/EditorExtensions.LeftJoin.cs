using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;

//using System.Data.Entity;
namespace Bns.Api.Common.Datatables.Backend;

public static class EditorLeftJoinExtensions
{
    public static Editor DbSetLeftJoin<T, D>(this Editor editor, DbSet<T> anotherTable, Expression<Func<T, object?>> anotherTableColumnName, DbSet<D> thisDbSet, Expression<Func<D, object?>> thisTableColumnName, string? op = "=", string? alias = null)
        where T : Entity
        where D : Entity
    {
        if (alias is null)
        {
            return editor.LeftJoin(anotherTable.GetTableNameWithSchema(), $"{anotherTable.GetTableNameWithSchema()}.{anotherTable.GetColumnName(anotherTableColumnName)}", op, $"{thisDbSet.GetTableNameWithSchema()}.{thisDbSet.GetColumnName(thisTableColumnName)}");
        }
        else
        {
            return editor.LeftJoin($"{anotherTable.GetTableNameWithSchema()} {alias}", $"{alias}.{anotherTable.GetColumnName(anotherTableColumnName)}", op, $"{thisDbSet.GetTableNameWithSchema()}.{thisDbSet.GetColumnName(thisTableColumnName)}");
        }
    }
}