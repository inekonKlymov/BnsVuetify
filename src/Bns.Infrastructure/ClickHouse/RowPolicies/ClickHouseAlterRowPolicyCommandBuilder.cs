namespace Bns.Infrastructure.ClickHouse.RowPolicies;

public class ClickHouseAlterRowPolicyCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<(string PolicyName, string? Cluster, string Table, string? RenameTo)> _policies = new();
    private string _asMode = string.Empty;
    private bool _forSelect = false;
    private readonly List<string> _usingConditions = new();
    private readonly List<string> _toRoles = new();
    private bool _toAll = false;
    private readonly List<string> _toAllExcept = new();
    private string _custom = string.Empty;

    public ClickHouseAlterRowPolicyCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterRowPolicyCommandBuilder AddPolicy(string policyName, string table, string? cluster = null, string? renameTo = null)
    {
        _policies.Add((policyName, cluster, table, renameTo));
        return this;
    }
    public ClickHouseAlterRowPolicyCommandBuilder AsMode(string mode) { _asMode = mode; return this; }
    public ClickHouseAlterRowPolicyCommandBuilder ForSelect(bool value = true) { _forSelect = value; return this; }
    public ClickHouseAlterRowPolicyCommandBuilder Using(params string[] conditions) { _usingConditions.AddRange(conditions); return this; }
    public ClickHouseAlterRowPolicyCommandBuilder ToRoles(params string[] roles) { _toRoles.AddRange(roles); return this; }
    public ClickHouseAlterRowPolicyCommandBuilder ToAll(bool value = true) { _toAll = value; return this; }
    public ClickHouseAlterRowPolicyCommandBuilder ToAllExcept(params string[] roles) { _toAllExcept.AddRange(roles); return this; }
    public ClickHouseAlterRowPolicyCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_policies.Any())
            throw new InvalidOperationException("At least one policy must be specified.");
        var sb = new System.Text.StringBuilder();
        sb.Append("ALTER ROW POLICY ");
        if (_ifExists) sb.Append("IF EXISTS ");
        for (int i = 0; i < _policies.Count; i++)
        {
            var (policyName, cluster, table, renameTo) = _policies[i];
            if (i > 0) sb.Append(", ");
            sb.Append(policyName);
            if (!string.IsNullOrWhiteSpace(cluster))
                sb.Append($" ON CLUSTER {cluster}");
            sb.Append($" ON {table}");
            if (!string.IsNullOrWhiteSpace(renameTo))
                sb.Append($" RENAME TO {renameTo}");
        }
        if (!string.IsNullOrWhiteSpace(_asMode))
            sb.Append($" AS {_asMode}");
        if (_forSelect)
            sb.Append(" FOR SELECT");
        if (_usingConditions.Any())
            sb.Append($" USING {string.Join(", ", _usingConditions)}");
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
