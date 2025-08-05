namespace Bns.Infrastructure.ClickHouse.Databases;

public class ClickHouseCreateDatabaseCommandBuilder : ClickHouseCommandBuilder
{
    private string _dbName = "";
    private bool _ifNotExists = false;
    private string _onCluster = "";
    private string _engine = "";
    private string _comment = "";
    private string _custom = "";

    public ClickHouseCreateDatabaseCommandBuilder Database(string dbName)
    {
        _dbName = dbName;
        return this;
    }
    public ClickHouseCreateDatabaseCommandBuilder IfNotExists(bool ifNotExists = true)
    {
        _ifNotExists = ifNotExists;
        return this;
    }
    public ClickHouseCreateDatabaseCommandBuilder OnCluster(string cluster)
    {
        _onCluster = cluster;
        return this;
    }
    public ClickHouseCreateDatabaseCommandBuilder Engine(string engine)
    {
        _engine = engine;
        return this;
    }
    public ClickHouseCreateDatabaseCommandBuilder Comment(string comment)
    {
        _comment = comment;
        return this;
    }
    public ClickHouseCreateDatabaseCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_dbName))
            throw new InvalidOperationException("Database name is required.");
        var query = "CREATE DATABASE ";
        if (_ifNotExists)
            query += "IF NOT EXISTS ";
        query += _dbName;
        if (!string.IsNullOrWhiteSpace(_onCluster))
            query += $" ON CLUSTER {_onCluster}";
        if (!string.IsNullOrWhiteSpace(_engine))
            query += $" ENGINE = {_engine}";
        if (!string.IsNullOrWhiteSpace(_comment))
            query += $" COMMENT '{_comment.Replace("'", "''")}'";
        if (!string.IsNullOrWhiteSpace(_custom))
            query += _custom;
        return query;
    }
}
