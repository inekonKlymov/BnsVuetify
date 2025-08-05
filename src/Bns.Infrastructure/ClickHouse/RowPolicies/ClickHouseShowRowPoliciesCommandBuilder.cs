namespace Bns.Infrastructure.ClickHouse.RowPolicies;

public class ClickHouseShowRowPoliciesCommandBuilder : ClickHouseCommandBuilder
{
    private string _table = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowRowPoliciesCommandBuilder OnTable(string table) { _table = table; return this; }
    public ClickHouseShowRowPoliciesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ROW POLICIES");
        if (!string.IsNullOrWhiteSpace(_table))
            sb.Append($" ON {_table}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
