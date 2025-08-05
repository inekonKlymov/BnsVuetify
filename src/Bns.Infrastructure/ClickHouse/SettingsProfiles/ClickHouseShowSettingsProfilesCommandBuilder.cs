namespace Bns.Infrastructure.ClickHouse.SettingsProfiles;

public class ClickHouseShowSettingsProfilesCommandBuilder : ClickHouseCommandBuilder
{
    private string _custom = string.Empty;

    public ClickHouseShowSettingsProfilesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW SETTINGS PROFILES");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
