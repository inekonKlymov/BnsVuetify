namespace Bns.Infrastructure.ClickHouse.Table.Alter;

public class ClickHouseAlterTableApplyDeletedMaskCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private string _inPartition = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterTableApplyDeletedMaskCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableApplyDeletedMaskCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableApplyDeletedMaskCommandBuilder InPartition(string partitionId) { _inPartition = partitionId; return this; }
    public ClickHouseAlterTableApplyDeletedMaskCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName))
            throw new InvalidOperationException("Table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append(" APPLY DELETED MASK");
        if (!string.IsNullOrWhiteSpace(_inPartition))
            sb.Append($" IN PARTITION {_inPartition}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
