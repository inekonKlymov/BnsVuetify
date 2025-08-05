using System.Text;
using System.Collections.Generic;
using System.Linq;
using Bns.Infrastructure.ClickHouse.Columns;

namespace Bns.Infrastructure.ClickHouse.Table;

public class ClickHouseCreateTableCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = "";
    private bool _ifNotExists = true;
    private string _onCluster = "";
    private readonly List<ClickHouseColumnDefinition> _columns = [];
    private string _engine = "MergeTree()";
    private string _orderBy = "";
    private string _tableComment = "";
    private string _custom = "";

    public ClickHouseCreateTableCommandBuilder Table(string tableName)
    {
        _tableName = tableName;
        return this;
    }
    public ClickHouseCreateTableCommandBuilder IfNotExists(bool ifNotExists = true)
    {
        _ifNotExists = ifNotExists;
        return this;
    }
    public ClickHouseCreateTableCommandBuilder OnCluster(string cluster)
    {
        _onCluster = cluster;
        return this;
    }
    public ClickHouseCreateTableCommandBuilder AddColumn(ClickHouseColumnDefinition column)
    {
        _columns.Add(column);
        return this;
    }
    public ClickHouseCreateTableCommandBuilder Columns(IEnumerable<ClickHouseColumnDefinition> columns)
    {
        _columns.AddRange(columns);
        return this;
    }
    public ClickHouseCreateTableCommandBuilder Engine(string engine)
    {
        _engine = engine;
        return this;
    }
    public ClickHouseCreateTableCommandBuilder OrderBy(string orderBy)
    {
        _orderBy = orderBy;
        return this;
    }
    public ClickHouseCreateTableCommandBuilder TableComment(string comment)
    {
        _tableComment = comment;
        return this;
    }
    public ClickHouseCreateTableCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || !_columns.Any())
            throw new InvalidOperationException("Table name and columns are required.");
        var sb = new StringBuilder();
        sb.Append("CREATE TABLE ");
        if (_ifNotExists)
            sb.Append("IF NOT EXISTS ");
        sb.Append(_tableName);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" (\n    {string.Join(",\n    ", _columns.Select(c => c.ToString()))}\n)");
        sb.Append($" ENGINE = {_engine}");
        if (!string.IsNullOrWhiteSpace(_orderBy))
            sb.Append($" ORDER BY {_orderBy}");
        if (!string.IsNullOrWhiteSpace(_tableComment))
            sb.Append($" COMMENT '{_tableComment.Replace("'", "''")}'");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
