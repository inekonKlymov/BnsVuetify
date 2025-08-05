namespace Bns.Infrastructure.ClickHouse.RowPolicies;

public class ClickHouseDropRowPolicyCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<(string PolicyName, string Table)> _policies = new();
    private string _onCluster = string.Empty;
    private string _fromAccessStorageType = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseDropRowPolicyCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropRowPolicyCommandBuilder AddPolicy(string policyName, string table)
    {
        _policies.Add((policyName, table));
        return this;
    }
    public ClickHouseDropRowPolicyCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropRowPolicyCommandBuilder FromAccessStorageType(string type) { _fromAccessStorageType = type; return this; }
    public ClickHouseDropRowPolicyCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_policies.Any())
            throw new InvalidOperationException("At least one policy and table must be specified.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP ROW POLICY ");
        if (_ifExists) sb.Append("IF EXISTS ");
        for (int i = 0; i < _policies.Count; i++)
        {
            var (policyName, table) = _policies[i];
            if (i > 0) sb.Append(", ");
            sb.Append(policyName);
            sb.Append($" ON {table}");
        }
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_fromAccessStorageType))
            sb.Append($" FROM {_fromAccessStorageType}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
