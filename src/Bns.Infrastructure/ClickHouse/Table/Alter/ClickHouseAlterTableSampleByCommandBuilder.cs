namespace Bns.Infrastructure.ClickHouse.Table.Alter;

public class ClickHouseAlterTableSampleByCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private string _newSampleBy = string.Empty;
    private bool _remove = false;
    private string _custom = string.Empty;

    public ClickHouseAlterTableSampleByCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableSampleByCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableSampleByCommandBuilder ModifySampleBy(string expr) { _newSampleBy = expr; _remove = false; return this; }
    public ClickHouseAlterTableSampleByCommandBuilder RemoveSampleBy() { _remove = true; _newSampleBy = string.Empty; return this; }
    public ClickHouseAlterTableSampleByCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName))
            throw new InvalidOperationException("Table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_remove)
            sb.Append(" REMOVE SAMPLE BY");
        else if (!string.IsNullOrWhiteSpace(_newSampleBy))
            sb.Append($" MODIFY SAMPLE BY {_newSampleBy}");
        else
            throw new InvalidOperationException("Either MODIFY SAMPLE BY or REMOVE SAMPLE BY must be specified.");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
