using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;

namespace Bns.Api.Common.Datatables.Backend;

public static class EditorFieldSearchBuilderExtensions
{
    public static Field DbSetSearchBuilderOptions<T>(this Field field, DbSet<T> table, Expression<Func<T, object?>> valueColumn, Expression<Func<T, object?>> labelColumn)
        where T : Entity
    {
        return field.SearchBuilderOptions(new SearchBuilderOptions().Table(table.GetTableNameWithSchema()).Value(table.GetColumnNameWithSchema(valueColumn)).Label(table.GetColumnNameWithSchema(labelColumn)));
    }

    public static Field DbSetSearchBuilderOptions<T>(this Field field, DbSet<T> table, Expression<Func<T, object?>> valueLabelColumn)
        where T : Entity
    {
        return field.SearchBuilderOptions(new SearchBuilderOptions().Table(table.GetTableNameWithSchema()).Value(table.GetColumnNameWithSchema(valueLabelColumn)).Label(table.GetColumnNameWithSchema(valueLabelColumn)));
    }

    public static Field DbSetSearchBuilderOptions<T, D>(this Field field, DbSet<T> mainTable, DbSet<D> anotherTable, Expression<Func<D, object?>> valueLabelColumn, string? aliasTableName = null)
        where T : Entity
        where D : Entity
    {
        return string.IsNullOrEmpty(aliasTableName)
            ? field.SearchBuilderOptions(
                new SearchBuilderOptions()
                .Table(mainTable.GetTableNameWithSchema())
                .Value(anotherTable.GetColumnNameWithSchema(valueLabelColumn))
                .Label(anotherTable.GetColumnNameWithSchema(valueLabelColumn)))
            : field.SearchBuilderOptions(
                new SearchBuilderOptions()
                .Table(mainTable.GetTableNameWithSchema())
                .Value($"{aliasTableName}.{anotherTable.GetColumnName(valueLabelColumn)}")
                .Label($"{aliasTableName}.{anotherTable.GetColumnName(valueLabelColumn)}"));
    }

    public static Field DbSetSearchBuilderOptions<T, D>(this Field field, DbSet<T> mainTable, DbSet<D> anotherTable, Expression<Func<D, object?>> valueColumn, Expression<Func<D, object?>> labelColumn, string? aliasTableName = null)
        where T : Entity
        where D : Entity
    {
        return string.IsNullOrEmpty(aliasTableName)
            ? field.SearchBuilderOptions(
                new SearchBuilderOptions()
                .Table(mainTable.GetTableNameWithSchema())
                .Value(anotherTable.GetColumnNameWithSchema(valueColumn))
                .Label(anotherTable.GetColumnNameWithSchema(labelColumn)))
            : field.SearchBuilderOptions(
                new SearchBuilderOptions()
                .Table(mainTable.GetTableNameWithSchema())
                .Value($"{aliasTableName}.{anotherTable.GetColumnName(valueColumn)}")
                .Label($"{aliasTableName}.{anotherTable.GetColumnName(labelColumn)}"));
    }
}