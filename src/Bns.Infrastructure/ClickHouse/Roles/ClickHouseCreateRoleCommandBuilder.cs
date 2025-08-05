namespace Bns.Infrastructure.ClickHouse.Roles;

public class ClickHouseCreateRoleCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifNotExists = false;
    private bool _orReplace = false;
    private readonly List<string> _roleNames = new();
    private string _onCluster = "";
    private string _accessStorageType = "";
    private readonly List<string> _settings = new();
    private string _custom = "";

    public ClickHouseCreateRoleCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseCreateRoleCommandBuilder OrReplace(bool value = true) { _orReplace = value; return this; }
    public ClickHouseCreateRoleCommandBuilder RoleNames(params string[] names) { _roleNames.AddRange(names); return this; }
    public ClickHouseCreateRoleCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseCreateRoleCommandBuilder AccessStorageType(string type) { _accessStorageType = type; return this; }
    public ClickHouseCreateRoleCommandBuilder Setting(string setting) { _settings.Add(setting); return this; }
    public ClickHouseCreateRoleCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_roleNames.Any())
            throw new InvalidOperationException("At least one role name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("CREATE ");
        if (_orReplace) sb.Append("OR REPLACE ");
        sb.Append("ROLE ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append(string.Join(", ", _roleNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_accessStorageType))
            sb.Append($" IN {_accessStorageType}");
        if (_settings.Any())
            sb.Append($" SETTINGS {string.Join(", ", _settings)}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
