namespace Bns.Infrastructure.ClickHouse.SettingsProfiles;

public class ClickHouseCreateSettingsProfileCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifNotExists = false;
    private bool _orReplace = false;
    private readonly List<string> _profileNames = new();
    private string _onCluster = string.Empty;
    private string _accessStorageType = string.Empty;
    private readonly List<string> _settings = new();
    private readonly List<string> _toRolesOrUsers = new();
    private bool _toAll = false;
    private bool _toNone = false;
    private readonly List<string> _toAllExcept = new();
    private string _custom = string.Empty;

    public ClickHouseCreateSettingsProfileCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder OrReplace(bool value = true) { _orReplace = value; return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder ProfileNames(params string[] names) { _profileNames.AddRange(names); return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder AccessStorageType(string type) { _accessStorageType = type; return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder Setting(string setting) { _settings.Add(setting); return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder To(params string[] rolesOrUsers) { _toRolesOrUsers.AddRange(rolesOrUsers); return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder ToAll(bool value = true) { _toAll = value; return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder ToNone(bool value = true) { _toNone = value; return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder ToAllExcept(params string[] except) { _toAllExcept.AddRange(except); return this; }
    public ClickHouseCreateSettingsProfileCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_profileNames.Any())
            throw new InvalidOperationException("At least one profile name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("CREATE SETTINGS PROFILE ");
        if (_orReplace) sb.Append("OR REPLACE ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append(string.Join(", ", _profileNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_accessStorageType))
            sb.Append($" IN {_accessStorageType}");
        if (_settings.Any())
            sb.Append($" SETTINGS {string.Join(", ", _settings)}");
        if (_toNone)
        {
            sb.Append(" TO NONE");
        }
        else if (_toAll)
        {
            sb.Append(" TO ALL");
            if (_toAllExcept.Any())
                sb.Append($" EXCEPT {string.Join(", ", _toAllExcept)}");
        }
        else if (_toRolesOrUsers.Any())
        {
            sb.Append($" TO {string.Join(", ", _toRolesOrUsers)}");
        }
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
