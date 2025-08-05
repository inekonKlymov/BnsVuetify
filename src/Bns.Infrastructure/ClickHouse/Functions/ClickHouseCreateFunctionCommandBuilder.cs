namespace Bns.Infrastructure.ClickHouse.Functions;

public class ClickHouseCreateFunctionCommandBuilder : ClickHouseCommandBuilder
{
    private string _functionName = "";
    private string _onCluster = "";
    private readonly List<string> _parameters = new();
    private string _expression = "";
    private string _custom = "";

    public ClickHouseCreateFunctionCommandBuilder Name(string functionName)
    {
        _functionName = functionName;
        return this;
    }
    public ClickHouseCreateFunctionCommandBuilder OnCluster(string cluster)
    {
        _onCluster = cluster;
        return this;
    }
    public ClickHouseCreateFunctionCommandBuilder Parameters(IEnumerable<string> parameters)
    {
        _parameters.Clear();
        _parameters.AddRange(parameters);
        return this;
    }
    public ClickHouseCreateFunctionCommandBuilder Expression(string expression)
    {
        _expression = expression;
        return this;
    }
    public ClickHouseCreateFunctionCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_functionName) || string.IsNullOrWhiteSpace(_expression))
            throw new InvalidOperationException("Function name and expression are required.");
        var query = $"CREATE FUNCTION {_functionName}";
        if (!string.IsNullOrWhiteSpace(_onCluster))
            query += $" ON CLUSTER {_onCluster}";
        query += " AS (" + string.Join(", ", _parameters) + ") -> " + _expression;
        if (!string.IsNullOrWhiteSpace(_custom))
            query += _custom;
        return query;
    }
}
