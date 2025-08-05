using Bns.Api.Common.Datatables.Backend;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Bns.Api.Common.Datatables.Backend;

public static class DbSetMetaColumnExtensions
{
    #region Public Methods

    public static string GetColumnName<T>(this DbSet<T> dbSet, Expression<Func<T, object?>> expression, bool addSquareBrackets = false) where T : class
    {
        var result = dbSet.EntityType.FindProperty(expression.GetPropertyInfo().Name).GetColumnName();
        return addSquareBrackets ? result.AddSquareBrackets() : result;
    }

    public static string GetColumnNameWithSchema<T>(this DbSet<T> dbSet, Expression<Func<T, object?>> expression, bool addSquareBrackets = false) where T : class
    {
        var result = $"{dbSet.GetTableNameWithSchema()}.{dbSet.GetColumnName(expression)}";
        return addSquareBrackets ? result.AddSquareBrackets() : result;
    }

    public static string GetColumnNameWithTable<T>(this DbSet<T> dbSet, Expression<Func<T, object?>> expression, bool addSquareBrackets = false) where T : class
    {
        var result = $"{dbSet.GetTableName()}.{dbSet.GetColumnName(expression)}";
        return addSquareBrackets ? result.AddSquareBrackets() : result;
    }

    public static string? GetTableName<T>(this DbSet<T> dbSet, bool addSquareBrackets = false) where T : class
    {
        var result = dbSet.EntityType.GetTableName();
        return addSquareBrackets ? result.AddSquareBrackets() : result;
    }

    public static string GetTableNameWithSchema<T>(this DbSet<T> dbSet, bool addSquareBrackets = false) where T : class
    {
        var schema = dbSet.GetSchemaName();
        string result = string.IsNullOrEmpty(schema) ? dbSet.EntityType.GetTableName() : $"{schema}.{dbSet.EntityType.GetTableName()}";
        return addSquareBrackets ? result.AddSquareBrackets() : result;
    }

    public static string GetSchemaName<T>(this DbSet<T> dbSet,  bool addSquareBrackets = false) where T : class
    {
        var result = dbSet.EntityType.GetAnnotation("Relational:Schema").Value?.ToString() ?? "" ;
        return addSquareBrackets ? result.AddSquareBrackets() : result;
    }

    public static IEnumerable<IKey> GetPrimaryKeys<T>(this DbSet<T> dbSet) where T : class => dbSet.EntityType.GetKeys().Where(s => s.IsPrimaryKey());

    public static string RemoveSquareBrackets(this string input) => Regex.Replace(input, "[^A-Za-z0-9 $_.-]", "");

    public static string AddSquareBrackets(this string input)
    {
        string[] parts = input.RemoveSquareBrackets().Split('.');
        List<string> newParts = new List<string>();
        foreach (var part in parts)
        {
            newParts.Add($"[{part}]");
        }
        return string.Join(".", newParts);
    }

    #endregion Public Methods
}