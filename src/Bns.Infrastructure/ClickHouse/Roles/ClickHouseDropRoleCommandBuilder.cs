namespace Bns.Infrastructure.ClickHouse.Roles;

public class ClickHouseDropRoleCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<string> _roleNames = new();
    private string _onCluster = string.Empty;
    private string _fromAccessStorageType = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseDropRoleCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropRoleCommandBuilder RoleNames(params string[] names) { _roleNames.AddRange(names); return this; }
    public ClickHouseDropRoleCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropRoleCommandBuilder FromAccessStorageType(string type) { _fromAccessStorageType = type; return this; }
    public ClickHouseDropRoleCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_roleNames.Any())
            throw new InvalidOperationException("At least one role name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP ROLE ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(string.Join(", ", _roleNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_fromAccessStorageType))
            sb.Append($" FROM {_fromAccessStorageType}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
