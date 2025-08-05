namespace Bns.Infrastructure.ClickHouse.Views;

public class ClickHouseCreateViewCommandBuilder : ClickHouseCommandBuilder
{
    private bool _orReplace = false;
    private bool _ifNotExists = false;
    private string _tableName = "";
    private readonly List<string> _aliases = new();
    private string _onCluster = "";
    private string _definer = "";
    private string _sqlSecurity = "";
    private string _selectQuery = "";
    private string _comment = "";
    private string _custom = "";

    public ClickHouseCreateViewCommandBuilder OrReplace(bool orReplace = true)
    {
        _orReplace = orReplace;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder IfNotExists(bool ifNotExists = true)
    {
        _ifNotExists = ifNotExists;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder Table(string tableName)
    {
        _tableName = tableName;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder Aliases(IEnumerable<string> aliases)
    {
        _aliases.Clear();
        _aliases.AddRange(aliases);
        return this;
    }
    public ClickHouseCreateViewCommandBuilder OnCluster(string cluster)
    {
        _onCluster = cluster;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder Definer(string definer)
    {
        _definer = definer;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder SqlSecurity(string sqlSecurity)
    {
        _sqlSecurity = sqlSecurity;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder AsSelect(string selectQuery)
    {
        _selectQuery = selectQuery;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder Comment(string comment)
    {
        _comment = comment;
        return this;
    }
    public ClickHouseCreateViewCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_selectQuery))
            throw new InvalidOperationException("Table name and SELECT query are required.");
        var query = "CREATE ";
        if (_orReplace)
            query += "OR REPLACE ";
        query += "VIEW ";
        if (_ifNotExists)
            query += "IF NOT EXISTS ";
        query += _tableName;
        if (_aliases.Any())
            query += $" ({string.Join(", ", _aliases)})";
        if (!string.IsNullOrWhiteSpace(_onCluster))
            query += $" ON CLUSTER {_onCluster}";
        if (!string.IsNullOrWhiteSpace(_definer))
            query += $" DEFINER = {_definer}";
        if (!string.IsNullOrWhiteSpace(_sqlSecurity))
            query += $" SQL SECURITY {_sqlSecurity}";
        query += $" AS {_selectQuery}";
        if (!string.IsNullOrWhiteSpace(_comment))
            query += $" COMMENT '{_comment.Replace("'", "''")}'";
        if (!string.IsNullOrWhiteSpace(_custom))
            query += _custom;
        return query;
    }
}
