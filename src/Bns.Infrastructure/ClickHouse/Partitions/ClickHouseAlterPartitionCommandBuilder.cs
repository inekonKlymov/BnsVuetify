namespace Bns.Infrastructure.ClickHouse.Partitions;

public class ClickHouseAlterPartitionCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private string _detachType = string.Empty; // PARTITION или PART
    private string _partitionExpr = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterPartitionCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterPartitionCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterPartitionCommandBuilder DetachPartition(string partitionExpr) { _detachType = "PARTITION"; _partitionExpr = partitionExpr; return this; }
    public ClickHouseAlterPartitionCommandBuilder DetachPart(string partExpr) { _detachType = "PART"; _partitionExpr = partExpr; return this; }
    public ClickHouseAlterPartitionCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_detachType) || string.IsNullOrWhiteSpace(_partitionExpr))
            throw new InvalidOperationException("Table name, detach type, and partition expression are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" DETACH {_detachType} {_partitionExpr}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
