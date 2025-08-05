namespace Bns.Infrastructure.ClickHouse.Table.Alter.Index;

public class ClickHouseAlterTableMaterializeIndexCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private bool _ifExists = false;
    private string _indexName = string.Empty;
    private string _inPartition = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterTableMaterializeIndexCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableMaterializeIndexCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableMaterializeIndexCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterTableMaterializeIndexCommandBuilder Name(string name) { _indexName = name; return this; }
    public ClickHouseAlterTableMaterializeIndexCommandBuilder InPartition(string partition) { _inPartition = partition; return this; }
    public ClickHouseAlterTableMaterializeIndexCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_indexName))
            throw new InvalidOperationException("Table name and index name are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append(" MATERIALIZE INDEX ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_indexName);
        if (!string.IsNullOrWhiteSpace(_inPartition))
            sb.Append($" IN PARTITION {_inPartition}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
