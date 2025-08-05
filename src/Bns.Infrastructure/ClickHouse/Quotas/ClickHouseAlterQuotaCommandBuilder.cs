namespace Bns.Infrastructure.ClickHouse.Quotas;

public class ClickHouseAlterQuotaCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private string _name = string.Empty;
    private string _onCluster = string.Empty;
    private string _renameTo = string.Empty;
    private string _keyedBy = string.Empty;
    private bool _notKeyed = false;
    private readonly List<string> _intervals = new();
    private readonly List<string> _toRoles = new();
    private bool _toAll = false;
    private readonly List<string> _toAllExcept = new();
    private string _custom = string.Empty;

    public ClickHouseAlterQuotaCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterQuotaCommandBuilder Name(string name) { _name = name; return this; }
    public ClickHouseAlterQuotaCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterQuotaCommandBuilder RenameTo(string newName) { _renameTo = newName; return this; }
    public ClickHouseAlterQuotaCommandBuilder KeyedBy(string keyedBy) { _keyedBy = keyedBy; _notKeyed = false; return this; }
    public ClickHouseAlterQuotaCommandBuilder NotKeyed(bool value = true) { _notKeyed = value; if (value) _keyedBy = string.Empty; return this; }
    public ClickHouseAlterQuotaCommandBuilder AddInterval(string intervalClause) { _intervals.Add(intervalClause); return this; }
    public ClickHouseAlterQuotaCommandBuilder ToRoles(params string[] roles) { _toRoles.AddRange(roles); return this; }
    public ClickHouseAlterQuotaCommandBuilder ToAll(bool value = true) { _toAll = value; return this; }
    public ClickHouseAlterQuotaCommandBuilder ToAllExcept(params string[] roles) { _toAllExcept.AddRange(roles); return this; }
    public ClickHouseAlterQuotaCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Quota name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("ALTER QUOTA ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_name);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_renameTo))
            sb.Append($" RENAME TO {_renameTo}");
        if (_notKeyed)
            sb.Append(" NOT KEYED");
        else if (!string.IsNullOrWhiteSpace(_keyedBy))
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
