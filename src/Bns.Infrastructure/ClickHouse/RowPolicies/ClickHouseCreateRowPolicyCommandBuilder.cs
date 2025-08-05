namespace Bns.Infrastructure.ClickHouse.RowPolicies;

public class ClickHouseCreateRowPolicyCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifNotExists = false;
    private bool _orReplace = false;
    private readonly List<(string PolicyName, string? Cluster, string Table)> _policies = new();
    private string _accessStorageType = "";
    private bool _forSelect = false;
    private string _usingCondition = "";
    private string _asMode = ""; // PERMISSIVE | RESTRICTIVE
    private readonly List<string> _toRoles = new();
    private bool _toAll = false;
    private readonly List<string> _toAllExcept = new();
    private string _custom = "";

    public ClickHouseCreateRowPolicyCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseCreateRowPolicyCommandBuilder OrReplace(bool value = true) { _orReplace = value; return this; }
    public ClickHouseCreateRowPolicyCommandBuilder AddPolicy(string policyName, string table, string? cluster = null)
    {
        _policies.Add((policyName, cluster, table));
        return this;
    }
    public ClickHouseCreateRowPolicyCommandBuilder AccessStorageType(string type) { _accessStorageType = type; return this; }
    public ClickHouseCreateRowPolicyCommandBuilder ForSelect(bool value = true) { _forSelect = value; return this; }
    public ClickHouseCreateRowPolicyCommandBuilder Using(string condition) { _usingCondition = condition; return this; }
    public ClickHouseCreateRowPolicyCommandBuilder AsMode(string mode) { _asMode = mode; return this; }
    public ClickHouseCreateRowPolicyCommandBuilder ToRoles(params string[] roles) { _toRoles.AddRange(roles); return this; }
    public ClickHouseCreateRowPolicyCommandBuilder ToAll(bool value = true) { _toAll = value; return this; }
    public ClickHouseCreateRowPolicyCommandBuilder ToAllExcept(params string[] roles) { _toAllExcept.AddRange(roles); return this; }
    public ClickHouseCreateRowPolicyCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_policies.Any())
            throw new InvalidOperationException("At least one policy must be specified.");
        var sb = new System.Text.StringBuilder();
        sb.Append("CREATE ROW POLICY ");
        if (_orReplace) sb.Append("OR REPLACE ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        for (int i = 0; i < _policies.Count; i++)
        {
            var (policyName, cluster, table) = _policies[i];
            if (i > 0) sb.Append(", ");
            sb.Append(policyName);
            if (!string.IsNullOrWhiteSpace(cluster))
                sb.Append($" ON CLUSTER {cluster}");
            sb.Append($" ON {table}");
        }
        if (!string.IsNullOrWhiteSpace(_accessStorageType))
            sb.Append($" IN {_accessStorageType}");
        if (_forSelect)
            sb.Append(" FOR SELECT");
        if (!string.IsNullOrWhiteSpace(_usingCondition))
            sb.Append($" USING {_usingCondition}");
        if (!string.IsNullOrWhiteSpace(_asMode))
            sb.Append($" AS {_asMode}");
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
