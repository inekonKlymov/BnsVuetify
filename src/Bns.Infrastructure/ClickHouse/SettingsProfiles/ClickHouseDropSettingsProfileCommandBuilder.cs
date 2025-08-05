namespace Bns.Infrastructure.ClickHouse.SettingsProfiles;

public class ClickHouseDropSettingsProfileCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<string> _profileNames = new();
    private string _onCluster = string.Empty;
    private string _fromAccessStorageType = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseDropSettingsProfileCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropSettingsProfileCommandBuilder ProfileNames(params string[] names) { _profileNames.AddRange(names); return this; }
    public ClickHouseDropSettingsProfileCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropSettingsProfileCommandBuilder FromAccessStorageType(string type) { _fromAccessStorageType = type; return this; }
    public ClickHouseDropSettingsProfileCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_profileNames.Any())
            throw new InvalidOperationException("At least one profile name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP SETTINGS PROFILE ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(string.Join(", ", _profileNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_fromAccessStorageType))
            sb.Append($" FROM {_fromAccessStorageType}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
