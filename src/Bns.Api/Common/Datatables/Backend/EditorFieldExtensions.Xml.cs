using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bns.Domain.Abstracts;
using DataTables;
using Microsoft.EntityFrameworkCore;

namespace Bns.Api.Common.Datatables.Backend;

public static class EditorFieldXmlExtensions
{
    public static readonly string XmlHeader = """<?xml version="1.0" encoding="utf-16"?>""";
    public static readonly string XmlNamespace = """ xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" """;
    private static readonly string _xmlAlias = "Alias";

    private static readonly string _xmlAliasValue = "val";

    public static Editor CreateFieldsForXmlColumn<TModel, TConfig>(this Editor editor, DbContext context, Expression<Func<TModel, object?>> idColumn, Expression<Func<TModel, object?>> configExpression, string cutsomFieldStartName = "")
        where TModel : Entity
        where TConfig : class, new()
    {
        return editor.CreateFieldsForXmlColumn<TModel, TConfig>(context, idColumn, configExpression, typeof(TConfig).GetProperties(), cutsomFieldStartName);
    }

    public static Editor CreateFieldsForXmlColumn<TModel, TConfig>(this Editor editor, DbContext context, Expression<Func<TModel, object?>> idColumn, Expression<Func<TModel, object?>> configExpression, IEnumerable<Expression<Func<TConfig, object?>>> configValueExpressions, string cutsomFieldStartName = "")
        where TModel : Entity
        where TConfig : class, new()
    {
        return editor.CreateFieldsForXmlColumn<TModel, TConfig>(context, idColumn, configExpression, configValueExpressions.Select(s => s.GetPropertyInfo()), cutsomFieldStartName);
    }

    private static Editor CreateFieldsForXmlColumn<TModel, TConfig>(this Editor editor, DbContext context, Expression<Func<TModel, object?>> idColumn, Expression<Func<TModel, object?>> configExpression, IEnumerable<PropertyInfo> propertiyInfoes, string cutsomFieldStartName = "")
        where TModel : Entity
        where TConfig : class, new()
    {
        uint _xmlAliasIndex = 0;
        var _db = context.Set<TModel>();

        foreach (var propertyInfo in propertiyInfoes)
        {
            string alias = $"{_xmlAlias}_{configExpression.GetPropertyInfo().Name}_{++_xmlAliasIndex}";
            var propType = propertyInfo.PropertyType;
            var xmlRootPath = typeof(TConfig).GetCustomAttribute<XmlRootAttribute>()?.ElementName ?? typeof(TConfig).Name;
            var XmlElementAttribute = propertyInfo.GetCustomAttribute<XmlElementAttribute>();
            var XmlArrayAttribute = propertyInfo.GetCustomAttribute<XmlArrayAttribute>();
            string fieldStartName = string.IsNullOrEmpty(cutsomFieldStartName) ? xmlRootPath : cutsomFieldStartName;
            if (propType.IsGenericType && typeof(List<>) == propType.GetGenericTypeDefinition() || XmlArrayAttribute is not null)
            {
                var GenericType = propType.GetGenericArguments()[0];
                var xmlPropPath = XmlArrayAttribute is not null ? XmlArrayAttribute.ElementName : propertyInfo.Name;
                var XmlArrayItemAttribute = propertyInfo.GetCustomAttribute<XmlArrayItemAttribute>();
                var xmlArrayItemPath = XmlArrayItemAttribute is not null ? XmlArrayItemAttribute.ElementName : GenericType.Name;
                editor
                    .Field(new Field($"{alias}.{_xmlAliasValue}", $"{fieldStartName}.{propertyInfo.Name}")
                        .Set(false)
                        .Type(propType)
                        .GetFormatter((val, row) =>
                        {
                            var xEl = val.ToString()
                                .Insert(propertyInfo.Name.Length + 1, XmlNamespace)
                                .Replace(propertyInfo.Name, $"ArrayOf{xmlArrayItemPath}")
                                ;
                            var sss = XElement.Parse(xEl.ToString());
                            MethodInfo method1 = typeof(SerializationHelper).GetMethod(nameof(SerializationHelper.DeserialiazeXElement));
                            MethodInfo generic1 = method1.MakeGenericMethod(propType);
                            var res1 = generic1.Invoke(null, [sss]);
                            return res1;
                            //var val4 = val.ToString()
                            //    .Insert(propertyInfo.Name.Length + 1, XmlNamespace)
                            //    .Replace(propertyInfo.Name, $"ArrayOf{xmlArrayItemPath}")
                            //    .Insert(0, XmlHeader )
                            //    ;
                            //MethodInfo method = typeof(SerializationService).GetMethod(nameof(SerializationService.Deserialize));
                            //MethodInfo generic = method.MakeGenericMethod(propType);
                            //var res = generic.Invoke(null, [val4]);
                        }))
                    .LeftJoin(
                    $"""
                        (Select {_db.GetColumnName(idColumn)} ,
                        CAST(CAST({_db.GetColumnName(configExpression)} AS XML).query('/{xmlRootPath}/{xmlPropPath}') AS NVARCHAR(MAX)) as  {_xmlAliasValue}
                        from {_db.GetTableNameWithSchema()}
                        )
                        {alias}
                    """,
                    $"""{alias}.{_db.GetColumnName(idColumn)} = {_db.GetColumnNameWithSchema(idColumn)}""");
            }
            else if (propType.IsClass && propType.Name != nameof(String))
            {
                throw new NotImplementedException();
                //var xmlPropPath = XmlElementAttribute is not null ? XmlElementAttribute.ElementName : propertyInfo.Name;
            }
            else
            {
                var xmlPropPath = XmlElementAttribute is not null ? XmlElementAttribute.ElementName : propertyInfo.Name;
                editor
                    .Field(new Field($"{alias}.{_xmlAliasValue}", $"{fieldStartName}.{propertyInfo.Name}")
                        .Set(false))
                    .LeftJoin(
                        $"""
                            (Select {_db.GetColumnName(idColumn)} ,
                            cast({_db.GetColumnName(configExpression)} as xml).value('({xmlRootPath}/{propertyInfo.Name})[1]', 'nvarchar(max)') as  {_xmlAliasValue}
                            from {_db.GetTableNameWithSchema()}
                            )
                            {alias}
                        """,
                        $"""{alias}.{_db.GetColumnName(idColumn)} = {_db.GetColumnNameWithSchema(idColumn)}""");
            }
        }
        return editor;
    }

    public static Editor CreateXmlFieldAsColumn<TModel, TConfig>(this Editor editor, DbSet<TModel> _db, Expression<Func<TModel, object?>> idColumn, Expression<Func<TModel, object?>> configExpression, Expression<Func<TConfig, object?>> configValueExpression, uint _xmlAliasIndex)
        where TModel : Entity
        where TConfig : class
    {
        string alias = $"{_xmlAlias}_{configExpression.GetPropertyInfo().Name}_{_xmlAliasIndex}";
        //string alias = $"{_xmlAlias}_{_xmlAliasIndex}";
        editor
            .Field(new Field($"{alias}.{_xmlAliasValue}", $"{_db.GetColumnNameWithTable(configExpression)}.{configValueExpression.GetPropertyInfo().Name}")
                .Set(Field.SetType.None))
            .LeftJoin(
                $"""
                    (Select {_db.GetColumnName(idColumn)} ,
                    cast({_db.GetColumnName(configExpression)} as xml).value('({typeof(TConfig).Name}/{configValueExpression.GetPropertyInfo().Name})[1]', 'nvarchar(max)') as  {_xmlAliasValue}
                    from {_db.GetTableNameWithSchema()}
                    )
                    {alias}
                """,
                $"""{alias}.{_db.GetColumnName(idColumn)} = {_db.GetColumnNameWithSchema(idColumn)}""");
        return editor;
    }

    public static Editor CreateXmlFieldAsColumn<TModel, TConfig, TXmlArrayModel, TXmlArray>(this Editor editor, DbSet<TModel> _db, Expression<Func<TModel, object?>> idColumn, Expression<Func<TModel, object?>> configExpression, Expression<Func<TConfig, TXmlArray?>> configValueExpression, uint _xmlAliasIndex)
        where TModel : Entity
        where TConfig : class
        where TXmlArray : IEnumerable<TXmlArrayModel>
        where TXmlArrayModel : class
    {
        string alias = $"{_xmlAlias}_{_xmlAliasIndex}";
        var propertyInfo = configValueExpression.GetPropertyInfo();
        editor
            .Field(new Field($"{alias}.{_xmlAliasValue}", $"{_db.GetColumnNameWithTable(configExpression)}.{propertyInfo.Name}")
                .Set(Field.SetType.None)
            .Type<List<TXmlArrayModel>>()
            .GetFormatter((val, row) =>
            {
                var val4 = val.ToString()
                .Insert(propertyInfo.Name.Length + 1, """ xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" """)
                .Replace(propertyInfo.Name, $"ArrayOf{typeof(TXmlArrayModel).Name}")
                .Insert(0, $"""
                <?xml version="1.0" encoding="utf-16"?>
                """);
                return SerializationHelper.Deserialize<List<TXmlArrayModel>>(val4);
            }))
            .LeftJoin(
            $"""
                (Select {_db.GetColumnName(idColumn)} ,
                CAST(CAST({_db.GetColumnName(configExpression)} AS XML).query('/{typeof(TConfig).Name}/{propertyInfo.Name}') AS NVARCHAR(MAX)) as  {_xmlAliasValue}
                from {_db.GetTableNameWithSchema()}
                )
                {alias}
            """,
            $"""{alias}.{_db.GetColumnName(idColumn)} = {_db.GetColumnNameWithSchema(idColumn)}""");
        return editor;
    }

    public static Field CreateXmlObjectFieldOptionsOnly<T>(Expression<Func<T, object?>> expression, string classAlias, List<Dictionary<string, object>> options) where T : class
    {
        return new Field($"{classAlias}.{expression.GetPropertyInfo().Name}")
                        .Get(false).Set(Field.SetType.None)
                        .Options(() => options);
    }
}