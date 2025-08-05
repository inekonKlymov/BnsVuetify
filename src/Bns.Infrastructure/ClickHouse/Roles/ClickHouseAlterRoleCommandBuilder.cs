namespace Bns.Infrastructure.ClickHouse.Roles;

public class ClickHouseAlterRoleCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<string> _roleNames = new();
    private string _renameTo = string.Empty;
    private string _onCluster = string.Empty;
    private bool _dropAllProfiles = false;
    private bool _dropAllSettings = false;
    private readonly List<string> _dropProfiles = new();
    private readonly List<string> _dropSettings = new();
    private readonly List<string> _addSettings = new();
    private readonly List<string> _modifySettings = new();
    private readonly List<string> _addProfiles = new();
    private string _custom = string.Empty;

    public ClickHouseAlterRoleCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterRoleCommandBuilder RoleNames(params string[] names) { _roleNames.AddRange(names); return this; }
    public ClickHouseAlterRoleCommandBuilder RenameTo(string newName) { _renameTo = newName; return this; }
    public ClickHouseAlterRoleCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterRoleCommandBuilder DropAllProfiles(bool value = true) { _dropAllProfiles = value; return this; }
    public ClickHouseAlterRoleCommandBuilder DropAllSettings(bool value = true) { _dropAllSettings = value; return this; }
    public ClickHouseAlterRoleCommandBuilder DropProfiles(params string[] profiles) { _dropProfiles.AddRange(profiles); return this; }
    public ClickHouseAlterRoleCommandBuilder DropSettings(params string[] settings) { _dropSettings.AddRange(settings); return this; }
    public ClickHouseAlterRoleCommandBuilder AddSettings(params string[] settings) { _addSettings.AddRange(settings); return this; }
    public ClickHouseAlterRoleCommandBuilder ModifySettings(params string[] settings) { _modifySettings.AddRange(settings); return this; }
    public ClickHouseAlterRoleCommandBuilder AddProfiles(params string[] profiles) { _addProfiles.AddRange(profiles); return this; }
    public ClickHouseAlterRoleCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_roleNames.Any())
            throw new InvalidOperationException("At least one role name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("ALTER ROLE ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(string.Join(", ", _roleNames));
        if (!string.IsNullOrWhiteSpace(_renameTo))
            sb.Append($" RENAME TO {_renameTo}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_dropAllProfiles)
            sb.Append(" DROP ALL PROFILES");
        if (_dropAllSettings)
            sb.Append(" DROP ALL SETTINGS");
        if (_dropSettings.Any())
            sb.Append($" DROP SETTINGS {string.Join(", ", _dropSettings)}");
        if (_dropProfiles.Any())
            sb.Append($" DROP PROFILES {string.Join(", ", _dropProfiles)}");
        if (_addSettings.Any())
            sb.Append($" ADD SETTINGS {string.Join(", ", _addSettings)}");
        if (_modifySettings.Any())
            sb.Append($" MODIFY SETTINGS {string.Join(", ", _modifySettings)}");
        if (_addProfiles.Any())
            sb.Append($" ADD PROFILES {string.Join(", ", _addProfiles)}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
