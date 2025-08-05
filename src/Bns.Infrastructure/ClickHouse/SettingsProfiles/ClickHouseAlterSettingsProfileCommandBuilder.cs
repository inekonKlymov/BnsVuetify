namespace Bns.Infrastructure.ClickHouse.SettingsProfiles;

public class ClickHouseAlterSettingsProfileCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<string> _profileNames = new();
    private string _renameTo = string.Empty;
    private string _onCluster = string.Empty;
    private bool _dropAllProfiles = false;
    private bool _dropAllSettings = false;
    private readonly List<string> _dropSettings = new();
    private readonly List<string> _dropProfiles = new();
    private readonly List<string> _addSettings = new();
    private readonly List<string> _modifySettings = new();
    private readonly List<string> _addProfiles = new();
    private readonly List<string> _toRolesOrUsers = new();
    private bool _toAll = false;
    private bool _toNone = false;
    private readonly List<string> _toAllExcept = new();
    private string _custom = string.Empty;

    public ClickHouseAlterSettingsProfileCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder ProfileNames(params string[] names) { _profileNames.AddRange(names); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder RenameTo(string newName) { _renameTo = newName; return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder DropAllProfiles(bool value = true) { _dropAllProfiles = value; return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder DropAllSettings(bool value = true) { _dropAllSettings = value; return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder DropSettings(params string[] settings) { _dropSettings.AddRange(settings); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder DropProfiles(params string[] profiles) { _dropProfiles.AddRange(profiles); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder AddSettings(params string[] settings) { _addSettings.AddRange(settings); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder ModifySettings(params string[] settings) { _modifySettings.AddRange(settings); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder AddProfiles(params string[] profiles) { _addProfiles.AddRange(profiles); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder To(params string[] rolesOrUsers) { _toRolesOrUsers.AddRange(rolesOrUsers); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder ToAll(bool value = true) { _toAll = value; return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder ToNone(bool value = true) { _toNone = value; return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder ToAllExcept(params string[] except) { _toAllExcept.AddRange(except); return this; }
    public ClickHouseAlterSettingsProfileCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_profileNames.Any())
            throw new InvalidOperationException("At least one profile name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("ALTER SETTINGS PROFILE ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(string.Join(", ", _profileNames));
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
