namespace Bns.Infrastructure.ClickHouse.Users;

public class ClickHouseShowAccessCommandBuilder : ClickHouseCommandBuilder
{
    private string _custom = string.Empty;

    public ClickHouseShowAccessCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ACCESS");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
