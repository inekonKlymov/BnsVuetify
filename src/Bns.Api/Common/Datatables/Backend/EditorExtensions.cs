using System.Linq.Expressions;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;

namespace Bns.Api.Common.Datatables.Backend;

public static class EditorExtensions
{
    public const string EditorActionCreate = "create";
    public const string EditorActionDraw = "draw";
    public const string EditorActionEdit = "edit";
    public const string EditorActionRemove = "remove";
    public const string EditorActionUpload = "upload";
    public const string EditorActionKey = "action";
    public const string SearchBuilderConditionHierarchy = "hierarchy";

    public static bool ContainHierarchySearchBuilder(this DtRequest request)
    {
        return ContainHierarchySearchBuilder(request.searchBuilder.criteria);
    }

    private static bool ContainHierarchySearchBuilder(List<SearchBuilderDetails> criterias)
    {
        return criterias.Any(criteria => criteria.condition == SearchBuilderConditionHierarchy || ContainHierarchySearchBuilder(criteria.criteria));
    }

    public static void ReplaceHierarchySearchBuilder(this DtRequest request)
    {
        ReplaceHierarchySearchBuilder(request.searchBuilder.criteria);
    }

    private static void ReplaceHierarchySearchBuilder(List<SearchBuilderDetails> criterias)
    {
        foreach (var criteria in criterias)
        {
            if (criteria.criteria.Count != 0) ReplaceHierarchySearchBuilder(criteria.criteria);
            if (criteria.condition == SearchBuilderConditionHierarchy)
            {
                var values = criteria.value1.Split(";");
                if (values.Count() == 1)
                {
                    criteria.condition = "contains";
                    criteria.type = "contains";
                    criteria.value1 = criteria.value1.Replace("[", "_").Replace("]", "_");
                }
                else
                {
                    var origData = criteria.origData;
                    var data = criteria.data;

                    criteria.criteria = values.Select(val => new SearchBuilderDetails()
                    {
                        data = data,
                        origData = origData,
                        condition = "contains",
                        criteria = [],
                        type = "html",
                        value1 = val.Replace("[", "_").Replace("]", "_"),
                        value2 = ""
                    }).ToList();
                    criteria.condition = null;
                    criteria.data = null;
                    criteria.origData = null;
                    criteria.type = null;
                    criteria.value1 = null;
                    criteria.value2 = null;

                    criteria.logic = "OR";
                }
            }
        }
    }

    public static Field GetField<T>(this Editor editor, DbContext db, Expression<Func<T, object?>> valueFunc) where T : Entity => editor.Field(db.Set<T>().GetColumnNameWithTable(valueFunc));

    public static Field GetField<T>(this Editor editor, DbSet<T> db, Expression<Func<T, object?>> valueFunc) where T : Entity => editor.Field(db.GetColumnNameWithTable(valueFunc));

    public static DtResponse LogErrors(this DtResponse response, ILogger logger)
    {
        if (!string.IsNullOrEmpty(response.error)) logger.LogWarning(response.error);
        response.fieldErrors.ForEach(s => logger.LogWarning($"{s.name} == {s.status}"));
        List<object>? debug = (response.debug as List<object>)?.Skip(1).ToList();
        debug.ForEach(s =>
        {
            var query = (s as DataTables.EditorUtil.DebugInfo).Query;
            logger.LogDebug(query);
        });
        return response;
    }

    public static T CreateObjectFromDictionary<T>(Dictionary<string, object> data, string classObjectName) where T : class, new()
    {
        var source = data[classObjectName] as Dictionary<string, object>;
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            someObjectType
                .GetProperty(item.Key)
                .SetValue(someObject, item.Value, null);
        }
        return someObject;
    }

    public static DataTablesAction GetDataTablesRequestAction(this HttpRequest request) => request.Form.Any(s => s.Key == EditorActionDraw)
            ? DataTablesAction.draw
            : request.Form.Any(s => s.Key == EditorActionKey)
                ? request.Form[EditorActionKey].ToString() switch
                {
                    EditorActionCreate => DataTablesAction.create,
                    EditorActionEdit => DataTablesAction.edit,
                    EditorActionRemove => DataTablesAction.remove,
                    EditorActionUpload => DataTablesAction.upload,
                    _ => DataTablesAction.unknown,
                }
                : DataTablesAction.none;

    #region Formatters

    public static List<Dictionary<string, object>> CreateOptions<T>(this IQueryable<T> query, Expression<Func<T, object?>> valueFunc, Expression<Func<T, object?>> labelFunc) where T : Entity
    {
        return query.Select(s => new Dictionary<string, object>() { { "value", valueFunc.Compile().Invoke(s) }, { "label", labelFunc.Compile().Invoke(s) } }).ToList();
    }

    public static Dictionary<string, object>? GetSubData(this Dictionary<string, object> data, string key)
    {
        return data[key] as Dictionary<string, object>;
    }

    public static class SearchBuilderOptionsPredefined
    {
        public static readonly List<Dictionary<string, dynamic>> Bool = [new() { { "value", 1 }, { "label", "true" } }, new() { { "value", 0 }, { "label", "false" } }];
    }

    #endregion Formatters

    #region Dbset

    public static Editor CreateEditor<T>(this DbSet<T> dbSet, Database db) where T : Entity
    {
        var tableName = dbSet.GetTableName();
        var schema = dbSet.GetSchemaName();
        var key = dbSet.EntityType.FindPrimaryKey();
        var pks = dbSet.EntityType.GetKeys().Where(s => s.IsPrimaryKey());

        string tableWithSchema = string.IsNullOrEmpty(schema) ? tableName : $"{schema}.{tableName}";
        var primaryKeys = pks.SelectMany(s => s.Properties.Select(s => $"{tableWithSchema}.{s.GetColumnName()}")).ToArray();

        var editor = new Editor(db, $"{tableWithSchema}", primaryKeys)
            .TryCatch(true)
            .Debug(true);
        return editor;
    }

    public static Editor CreateEditorPkSimple<T>(this DbSet<T> dbSet, Database db) where T : Entity
    {
        var tableName = dbSet.GetTableNameWithSchema();
        var primaryKeys = dbSet.EntityType.GetKeys().Where(s => s.IsPrimaryKey()).SelectMany(s => s.Properties.Select(s => s.Name)).ToArray();
        var editor = new Editor(db, tableName, primaryKeys)
            .TryCatch(true)
            .Debug(true);
        return editor;
    }

    #endregion Dbset
}

public enum DataTablesAction
{
    draw,
    create,
    edit,
    remove,
    upload,
    none,
    unknown
}