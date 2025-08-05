namespace Bns.Infrastructure.ClickHouse.Settings;

public class ClickHouseShowSettingsCommandBuilder : ClickHouseCommandBuilder
{
    private bool _changed = false;
    private string _like = string.Empty;
    private string _iLike = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowSettingsCommandBuilder Changed(bool value = true) { _changed = value; return this; }
    public ClickHouseShowSettingsCommandBuilder Like(string pattern) { _like = pattern; _iLike = string.Empty; return this; }
    public ClickHouseShowSettingsCommandBuilder ILike(string pattern) { _iLike = pattern; _like = string.Empty; return this; }
    public ClickHouseShowSettingsCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ");
        if (_changed) sb.Append("CHANGED ");
        sb.Append("SETTINGS");
        if (!string.IsNullOrWhiteSpace(_like))
            sb.Append($" LIKE '{_like}'");
        else if (!string.IsNullOrWhiteSpace(_iLike))
            sb.Append($" ILIKE '{_iLike}'");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
