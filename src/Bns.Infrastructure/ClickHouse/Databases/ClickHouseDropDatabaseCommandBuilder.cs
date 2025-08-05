namespace Bns.Infrastructure.ClickHouse.Databases;

public class ClickHouseDropDatabaseCommandBuilder : ClickHouseCommandBuilder
{
    private string _dbName = string.Empty;
    private bool _ifExists = false;
    private string _onCluster = string.Empty;
    private bool _sync = false;
    private string _custom = string.Empty;

    public ClickHouseDropDatabaseCommandBuilder Database(string dbName) { _dbName = dbName; return this; }
    public ClickHouseDropDatabaseCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropDatabaseCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropDatabaseCommandBuilder Sync(bool value = true) { _sync = value; return this; }
    public ClickHouseDropDatabaseCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_dbName))
            throw new InvalidOperationException("Database name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP DATABASE ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_dbName);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_sync)
            sb.Append(" SYNC");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
