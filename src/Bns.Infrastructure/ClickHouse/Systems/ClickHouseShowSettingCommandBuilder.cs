namespace Bns.Infrastructure.ClickHouse.Settings;

public class ClickHouseShowSettingCommandBuilder : ClickHouseCommandBuilder
{
    private string _name = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowSettingCommandBuilder Name(string name) { _name = name; return this; }
    public ClickHouseShowSettingCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Setting name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW SETTING ");
        sb.Append(_name);
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
