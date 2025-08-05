namespace Bns.Infrastructure.ClickHouse.Functions;

public class ClickHouseDropFunctionCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private string _functionName = string.Empty;
    private string _onCluster = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseDropFunctionCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropFunctionCommandBuilder Name(string functionName) { _functionName = functionName; return this; }
    public ClickHouseDropFunctionCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropFunctionCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_functionName))
            throw new InvalidOperationException("Function name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP FUNCTION ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_functionName);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
