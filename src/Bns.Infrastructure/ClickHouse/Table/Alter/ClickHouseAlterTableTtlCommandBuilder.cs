namespace Bns.Infrastructure.ClickHouse.Table.Alter;

public class ClickHouseAlterTableTtlCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private string _ttlExpression = string.Empty;
    private bool _remove = false;
    private string _custom = string.Empty;

    public ClickHouseAlterTableTtlCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableTtlCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableTtlCommandBuilder ModifyTtl(string expr) { _ttlExpression = expr; _remove = false; return this; }
    public ClickHouseAlterTableTtlCommandBuilder RemoveTtl() { _remove = true; _ttlExpression = string.Empty; return this; }
    public ClickHouseAlterTableTtlCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName))
            throw new InvalidOperationException("Table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_remove)
            sb.Append(" REMOVE TTL");
        else if (!string.IsNullOrWhiteSpace(_ttlExpression))
            sb.Append($" MODIFY TTL {_ttlExpression}");
        else
            throw new InvalidOperationException("Either MODIFY TTL or REMOVE TTL must be specified.");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
