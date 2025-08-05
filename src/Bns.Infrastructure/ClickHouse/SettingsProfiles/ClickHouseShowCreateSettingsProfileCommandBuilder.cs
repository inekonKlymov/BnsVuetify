namespace Bns.Infrastructure.ClickHouse.SettingsProfiles;

public class ClickHouseShowCreateSettingsProfileCommandBuilder : ClickHouseCommandBuilder
{
    private readonly List<string> _profileNames = new();
    private string _custom = string.Empty;

    public ClickHouseShowCreateSettingsProfileCommandBuilder ProfileNames(params string[] names) { _profileNames.AddRange(names); return this; }
    public ClickHouseShowCreateSettingsProfileCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_profileNames.Any())
            throw new InvalidOperationException("At least one profile name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW CREATE SETTINGS PROFILE ");
        sb.Append(string.Join(", ", _profileNames));
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
