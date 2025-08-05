namespace Bns.Infrastructure.ClickHouse.RowPolicies;

public class ClickHouseShowCreateRowPolicyCommandBuilder : ClickHouseCommandBuilder
{
    private string _policyName = string.Empty;
    private readonly List<string> _tables = new();
    private string _custom = string.Empty;

    public ClickHouseShowCreateRowPolicyCommandBuilder PolicyName(string name) { _policyName = name; return this; }
    public ClickHouseShowCreateRowPolicyCommandBuilder Tables(params string[] tables) { _tables.AddRange(tables); return this; }
    public ClickHouseShowCreateRowPolicyCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_policyName) || !_tables.Any())
            throw new InvalidOperationException("Policy name and at least one table are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW CREATE ROW POLICY ");
        sb.Append(_policyName);
        sb.Append(" ON ");
        sb.Append(string.Join(", ", _tables));
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
