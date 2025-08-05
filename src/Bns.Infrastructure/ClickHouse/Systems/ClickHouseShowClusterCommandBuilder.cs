namespace Bns.Infrastructure.ClickHouse.Systems;

public class ClickHouseShowClusterCommandBuilder : ClickHouseCommandBuilder
{
    private string _name = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowClusterCommandBuilder Name(string name) { _name = name; return this; }
    public ClickHouseShowClusterCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Cluster name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW CLUSTER '");
        sb.Append(_name.Replace("'", "''"));
        sb.Append("'");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
