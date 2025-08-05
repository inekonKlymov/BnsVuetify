namespace Bns.Infrastructure.ClickHouse.Table.Alter;

public class ClickHouseAlterTableDeleteWhereCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private string _filterExpr = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterTableDeleteWhereCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableDeleteWhereCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableDeleteWhereCommandBuilder Filter(string filterExpr) { _filterExpr = filterExpr; return this; }
    public ClickHouseAlterTableDeleteWhereCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_filterExpr))
            throw new InvalidOperationException("Table name and filter expression are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" DELETE WHERE {_filterExpr}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
