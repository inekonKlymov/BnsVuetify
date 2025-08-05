using Bns.Domain.Common.Startup;
using ClickHouse.Client.ADO;
using Microsoft.Extensions.Options;

namespace Bns.Infrastructure.ClickHouse;
public class ClickHouseConnecter(IOptions<AppSettings> options)
{
    private readonly string _connectionString = options.Value.ConnectionStrings.ClickHouse;

    public object ExecuteCommand(ClickHouseCommandBuilder command)
    {
        using ClickHouseConnection connection = new (_connectionString);
        connection.Open();
        using ClickHouseCommand cmd = connection.CreateCommand();
        cmd.CommandText = command.Build();
        object result = cmd.ExecuteScalar();
        connection.Close();
        return result;
    }
}

public class ClickHouseUpdateRowCommandBuilder : ClickHouseCommandBuilder
{
    private string _updateTable = "";
    private string _setClause = "";
    private string _where = "";
    private string _custom = "";
    public ClickHouseUpdateRowCommandBuilder Table(string table)
    {
        _updateTable = table;
        return this;
    }
    public ClickHouseUpdateRowCommandBuilder Set(string setClause)
    {
        _setClause = setClause;
        return this;
    }
    public ClickHouseUpdateRowCommandBuilder Where(string condition)
    {
        _where = condition;
        return this;
    }
    public ClickHouseUpdateRowCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_updateTable) || string.IsNullOrWhiteSpace(_setClause))
            throw new InvalidOperationException("UPDATE table and SET clause are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"UPDATE {_updateTable} SET {_setClause}");
        if (!string.IsNullOrWhiteSpace(_where))
            sb.Append($" WHERE {_where}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}

public class ClickHouseDeleteRowCommandBuilder : ClickHouseCommandBuilder
{
    private string _deleteTable = "";
    private string _where = "";
    private string _custom = "";
    public ClickHouseDeleteRowCommandBuilder From(string table)
    {
        _deleteTable = table;
        return this;
    }
    public ClickHouseDeleteRowCommandBuilder Where(string condition)
    {
        _where = condition;
        return this;
    }
    public ClickHouseDeleteRowCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_deleteTable))
            throw new InvalidOperationException("DELETE FROM table is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"DELETE FROM {_deleteTable}");
        if (!string.IsNullOrWhiteSpace(_where))
            sb.Append($" WHERE {_where}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}

public class ClickHouseShowTablesCommandBuilder : ClickHouseCommandBuilder
{
    private string _database = "";
    public ClickHouseShowTablesCommandBuilder FromDatabase(string database)
    {
        _database = database;
        return this;
    }
    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        if (string.IsNullOrWhiteSpace(_database))
            sb.Append("SHOW TABLES");
        else
            sb.Append($"SHOW TABLES FROM {_database}");
        return sb.ToString();
    }
}

public class ClickHouseDescribeTableCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = "";
    public ClickHouseDescribeTableCommandBuilder Table(string tableName)
    {
        _tableName = tableName;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName))
            throw new InvalidOperationException("Table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"DESCRIBE TABLE {_tableName}");
        return sb.ToString();
    }
}

public class ClickHouseShowDatabasesCommandBuilder : ClickHouseCommandBuilder
{
    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW DATABASES");
        return sb.ToString();
    }
}

public class ClickHouseCurrentDatabaseCommandBuilder : ClickHouseCommandBuilder
{
    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SELECT currentDatabase()");
        return sb.ToString();
    }
}

public class ClickHouseShowCreateTableCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = "";
    public ClickHouseShowCreateTableCommandBuilder Table(string tableName)
    {
        _tableName = tableName;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName))
            throw new InvalidOperationException("Table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"SHOW CREATE TABLE {_tableName}");
        return sb.ToString();
    }
}

public class ClickHouseSystemTableSelectCommandBuilder : ClickHouseCommandBuilder
{
    private string _table = "";
    private string _where = "";
    private string _columns = "*";
    public ClickHouseSystemTableSelectCommandBuilder Table(string table)
    {
        _table = table;
        return this;
    }
    public ClickHouseSystemTableSelectCommandBuilder Where(string where)
    {
        _where = where;
        return this;
    }
    public ClickHouseSystemTableSelectCommandBuilder Columns(string columns)
    {
        _columns = columns;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_table))
            throw new InvalidOperationException("System table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"SELECT {_columns} FROM system.{_table}");
        if (!string.IsNullOrWhiteSpace(_where))
            sb.Append($" WHERE {_where}");
        return sb.ToString();
    }
}
