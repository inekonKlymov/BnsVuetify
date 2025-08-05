namespace Bns.Infrastructure.ClickHouse.Table.Alter;

public class ClickHouseAlterTableUpdateCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private readonly List<string> _setClauses = new();
    private string _inPartition = string.Empty;
    private string _where = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterTableUpdateCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableUpdateCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableUpdateCommandBuilder Set(string setClause) { _setClauses.Add(setClause); return this; }
    public ClickHouseAlterTableUpdateCommandBuilder InPartition(string partitionId) { _inPartition = partitionId; return this; }
    public ClickHouseAlterTableUpdateCommandBuilder Where(string where) { _where = where; return this; }
    public ClickHouseAlterTableUpdateCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || !_setClauses.Any() || string.IsNullOrWhiteSpace(_where))
            throw new InvalidOperationException("Table name, at least one SET clause, and WHERE are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" UPDATE {string.Join(", ", _setClauses)}");
        if (!string.IsNullOrWhiteSpace(_inPartition))
            sb.Append($" IN PARTITION {_inPartition}");
        sb.Append($" WHERE {_where}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
