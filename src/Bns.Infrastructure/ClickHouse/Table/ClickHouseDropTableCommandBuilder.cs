namespace Bns.Infrastructure.ClickHouse.Table;

public class ClickHouseDropTableCommandBuilder : ClickHouseCommandBuilder
{
    private bool _temporary = false;
    private bool _ifExists = true;
    private bool _ifEmpty = false;
    private string _onCluster = string.Empty;
    private bool _sync = false;
    private readonly List<string> _tableNames = new();
    private string _custom = string.Empty;

    public ClickHouseDropTableCommandBuilder Temporary(bool value = true) { _temporary = value; return this; }
    public ClickHouseDropTableCommandBuilder IfExists(bool ifExists = true) { _ifExists = ifExists; return this; }
    public ClickHouseDropTableCommandBuilder IfEmpty(bool value = true) { _ifEmpty = value; return this; }
    public ClickHouseDropTableCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropTableCommandBuilder Sync(bool value = true) { _sync = value; return this; }
    public ClickHouseDropTableCommandBuilder Table(string tableName) { _tableNames.Clear(); _tableNames.Add(tableName); return this; }
    public ClickHouseDropTableCommandBuilder Tables(params string[] tableNames) { _tableNames.Clear(); _tableNames.AddRange(tableNames); return this; }
    public ClickHouseDropTableCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_tableNames.Any())
            throw new InvalidOperationException("At least one table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP ");
        if (_temporary) sb.Append("TEMPORARY ");
        sb.Append("TABLE ");
        if (_ifExists) sb.Append("IF EXISTS ");
        if (_ifEmpty) sb.Append("IF EMPTY ");
        sb.Append(string.Join(", ", _tableNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_sync) sb.Append(" SYNC");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
