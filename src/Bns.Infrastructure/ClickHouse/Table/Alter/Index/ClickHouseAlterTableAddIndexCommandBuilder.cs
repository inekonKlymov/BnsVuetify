namespace Bns.Infrastructure.ClickHouse.Table.Alter.Index;

public class ClickHouseAlterTableAddIndexCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private bool _ifNotExists = false;
    private string _indexName = string.Empty;
    private string _expression = string.Empty;
    private string _type = string.Empty;
    private string _granularity = string.Empty;
    private string _position = string.Empty; // FIRST|AFTER name
    private string _custom = string.Empty;

    public ClickHouseAlterTableAddIndexCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder Name(string name) { _indexName = name; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder Expression(string expr) { _expression = expr; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder Type(string type) { _type = type; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder Granularity(string value) { _granularity = value; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder First() { _position = "FIRST"; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder After(string name) { _position = $"AFTER {name}"; return this; }
    public ClickHouseAlterTableAddIndexCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_indexName) || string.IsNullOrWhiteSpace(_expression) || string.IsNullOrWhiteSpace(_type))
            throw new InvalidOperationException("Table name, index name, expression, and type are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append(" ADD INDEX ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append($"{_indexName} {_expression} TYPE {_type}");
        if (!string.IsNullOrWhiteSpace(_granularity))
            sb.Append($" GRANULARITY {_granularity}");
        if (!string.IsNullOrWhiteSpace(_position))
            sb.Append($" {_position}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
