namespace Bns.Infrastructure.ClickHouse.Table.Alter.Statistics;

public class ClickHouseAlterTableDropStatisticsCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private bool _ifExists = false;
    private List<string> _columns = new();
    private string _custom = string.Empty;

    public ClickHouseAlterTableDropStatisticsCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableDropStatisticsCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableDropStatisticsCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterTableDropStatisticsCommandBuilder Columns(params string[] columns) { _columns.AddRange(columns); return this; }
    public ClickHouseAlterTableDropStatisticsCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || !_columns.Any())
            throw new InvalidOperationException("Table name and columns are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append(" DROP STATISTICS ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append($"({string.Join(", ", _columns)})");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
