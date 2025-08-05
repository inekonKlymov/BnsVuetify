namespace Bns.Infrastructure.ClickHouse.Quotas;

public class ClickHouseShowQuotasCommandBuilder : ClickHouseCommandBuilder
{
    private string _custom = string.Empty;

    public ClickHouseShowQuotasCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW QUOTAS");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
