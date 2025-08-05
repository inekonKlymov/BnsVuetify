namespace Bns.Infrastructure.ClickHouse.Quotas;

public class ClickHouseShowQuotaCommandBuilder : ClickHouseCommandBuilder
{
    private bool _current = false;
    private string _custom = string.Empty;

    public ClickHouseShowQuotaCommandBuilder Current(bool value = true) { _current = value; return this; }
    public ClickHouseShowQuotaCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ");
        if (_current)
            sb.Append("CURRENT ");
        sb.Append("QUOTA");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
