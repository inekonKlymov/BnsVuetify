namespace Bns.Infrastructure.ClickHouse.Quotas;

public class ClickHouseShowCreateQuotaCommandBuilder : ClickHouseCommandBuilder
{
    private readonly List<string> _quotaNames = new();
    private bool _current = false;
    private string _custom = string.Empty;

    public ClickHouseShowCreateQuotaCommandBuilder QuotaNames(params string[] names) { _quotaNames.AddRange(names); _current = false; return this; }
    public ClickHouseShowCreateQuotaCommandBuilder Current(bool value = true) { _current = value; if (value) _quotaNames.Clear(); return this; }
    public ClickHouseShowCreateQuotaCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW CREATE QUOTA");
        if (_current)
        {
            sb.Append(" CURRENT");
        }
        else if (_quotaNames.Any())
        {
            sb.Append($" {string.Join(", ", _quotaNames)}");
        }
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
