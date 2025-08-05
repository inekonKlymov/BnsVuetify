namespace Bns.Infrastructure.ClickHouse.Users;

public class ClickHouseDropUserCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<string> _userNames = new();
    private string _onCluster = string.Empty;
    private string _fromAccessStorageType = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseDropUserCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropUserCommandBuilder UserNames(params string[] names) { _userNames.AddRange(names); return this; }
    public ClickHouseDropUserCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropUserCommandBuilder FromAccessStorageType(string type) { _fromAccessStorageType = type; return this; }
    public ClickHouseDropUserCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_userNames.Any())
            throw new InvalidOperationException("At least one user name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP USER ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(string.Join(", ", _userNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_fromAccessStorageType))
            sb.Append($" FROM {_fromAccessStorageType}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
