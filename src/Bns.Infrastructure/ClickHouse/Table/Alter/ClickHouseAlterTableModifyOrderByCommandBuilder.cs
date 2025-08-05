namespace Bns.Infrastructure.ClickHouse.Table.Alter;

public class ClickHouseAlterTableModifyOrderByCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private string _newOrderBy = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterTableModifyOrderByCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableModifyOrderByCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableModifyOrderByCommandBuilder NewOrderBy(string orderBy) { _newOrderBy = orderBy; return this; }
    public ClickHouseAlterTableModifyOrderByCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_newOrderBy))
            throw new InvalidOperationException("Table name and new ORDER BY expression are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" MODIFY ORDER BY {_newOrderBy}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
