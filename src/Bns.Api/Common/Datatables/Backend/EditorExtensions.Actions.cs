using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;

//using System.Data.Entity;
namespace Bns.Api.Common.Datatables.Backend;

public record DatatablesEditorDbSetCondition<T>(Expression<Func<T, object>> expression, dynamic value) where T : Entity;

public static class EditorActionsExtensions
{
    public static Result DeleteDbSet<T>(this Database db, DbSet<T> dbSet, List<DatatablesEditorDbSetCondition<T>> values) where T : Entity
    {
        var conditions = values.ToDictionary(s => dbSet.GetColumnName(s.expression), s => s.value);
        return db.Delete(dbSet.GetTableName(), conditions);
    }

    public static Result UpdateDbSet<T>(this Database db, DbSet<T> dbSet, List<DatatablesEditorDbSetCondition<T>> sets, List<DatatablesEditorDbSetCondition<T>> wheres) where T : Entity
    {
        var set = sets.ToDictionary(s => dbSet.GetColumnName(s.expression), s => s.value);
        var where = wheres.ToDictionary(s => dbSet.GetColumnName(s.expression), s => s.value);
        return db.Update(dbSet.GetTableName(), set, where);
    }

    public static Result InsertDbSet<T>(this Database db, DbSet<T> dbSet, List<DatatablesEditorDbSetCondition<T>> sets, string[] pkeys = null) where T : Entity
    {
        var set = sets.ToDictionary(s => dbSet.GetColumnName(s.expression), s => s.value);
        var tableName = dbSet.GetTableName();
        pkeys ??= [.. dbSet.GetPrimaryKeys().SelectMany(s => s.Properties.Select(s => $"{tableName}.{s.Name}"))];
        return db.Insert(tableName, set, pkeys);
    }
}