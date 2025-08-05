namespace Bns.Infrastructure.ClickHouse.Quotas;

public class ClickHouseDropQuotaCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<string> _quotaNames = new();
    private string _onCluster = string.Empty;
    private string _fromAccessStorageType = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseDropQuotaCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropQuotaCommandBuilder QuotaNames(params string[] names) { _quotaNames.AddRange(names); return this; }
    public ClickHouseDropQuotaCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropQuotaCommandBuilder FromAccessStorageType(string type) { _fromAccessStorageType = type; return this; }
    public ClickHouseDropQuotaCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_quotaNames.Any())
            throw new InvalidOperationException("At least one quota name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP QUOTA ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(string.Join(", ", _quotaNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_fromAccessStorageType))
            sb.Append($" FROM {_fromAccessStorageType}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
