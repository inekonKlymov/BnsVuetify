namespace Bns.Infrastructure.ClickHouse.Table.Alter.Statistics;

public class ClickHouseAlterTableModifyStatisticsCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private List<string> _columns = new();
    private List<string> _types = new();
    private string _custom = string.Empty;

    public ClickHouseAlterTableModifyStatisticsCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableModifyStatisticsCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableModifyStatisticsCommandBuilder Columns(params string[] columns) { _columns.AddRange(columns); return this; }
    public ClickHouseAlterTableModifyStatisticsCommandBuilder Types(params string[] types) { _types.AddRange(types); return this; }
    public ClickHouseAlterTableModifyStatisticsCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || !_columns.Any() || !_types.Any())
            throw new InvalidOperationException("Table name, columns, and types are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" MODIFY STATISTICS ({string.Join(", ", _columns)}) TYPE ({string.Join(", ", _types)})");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
