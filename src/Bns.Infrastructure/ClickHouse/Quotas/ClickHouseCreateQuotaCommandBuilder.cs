namespace Bns.Infrastructure.ClickHouse.Quotas;

public class ClickHouseCreateQuotaCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifNotExists = false;
    private bool _orReplace = false;
    private string _name = string.Empty;
    private string _onCluster = string.Empty;
    private string _accessStorageType = string.Empty;
    private string _keyedBy = string.Empty; // либо NOT KEYED, либо перечисление ключей
    private readonly List<string> _intervals = new(); // каждый FOR ... INTERVAL ...
    private readonly List<string> _toRoles = new();
    private bool _toAll = false;
    private readonly List<string> _toAllExcept = new();
    private string _custom = string.Empty;

    public ClickHouseCreateQuotaCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseCreateQuotaCommandBuilder OrReplace(bool value = true) { _orReplace = value; return this; }
    public ClickHouseCreateQuotaCommandBuilder Name(string name) { _name = name; return this; }
    public ClickHouseCreateQuotaCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseCreateQuotaCommandBuilder AccessStorageType(string type) { _accessStorageType = type; return this; }
    public ClickHouseCreateQuotaCommandBuilder KeyedBy(string keyedBy) { _keyedBy = keyedBy; return this; }
    public ClickHouseCreateQuotaCommandBuilder NotKeyed() { _keyedBy = "NOT KEYED"; return this; }
    public ClickHouseCreateQuotaCommandBuilder AddInterval(string intervalClause) { _intervals.Add(intervalClause); return this; }
    public ClickHouseCreateQuotaCommandBuilder ToRoles(params string[] roles) { _toRoles.AddRange(roles); return this; }
    public ClickHouseCreateQuotaCommandBuilder ToAll(bool value = true) { _toAll = value; return this; }
    public ClickHouseCreateQuotaCommandBuilder ToAllExcept(params string[] roles) { _toAllExcept.AddRange(roles); return this; }
    public ClickHouseCreateQuotaCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Quota name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("CREATE ");
        if (_orReplace) sb.Append("OR REPLACE ");
        sb.Append("QUOTA ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append(_name);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_accessStorageType))
            sb.Append($" IN {_accessStorageType}");
        if (!string.IsNullOrWhiteSpace(_keyedBy))
            sb.Append($" KEYED BY {_keyedBy}");
        foreach (var interval in _intervals)
            sb.Append($" FOR {interval}");
        if (_toAll)
        {
            sb.Append(" TO ALL");
            if (_toAllExcept.Any())
                sb.Append($" EXCEPT {string.Join(", ", _toAllExcept)}");
        }
        else if (_toRoles.Any())
        {
            sb.Append($" TO {string.Join(", ", _toRoles)}");
        }
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
