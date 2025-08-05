namespace Bns.Infrastructure.ClickHouse.Roles;

public class ClickHouseShowRolesCommandBuilder : ClickHouseCommandBuilder
{
    private bool _current = false;
    private bool _enabled = false;
    private string _custom = string.Empty;

    public ClickHouseShowRolesCommandBuilder Current(bool value = true) { _current = value; if (value) _enabled = false; return this; }
    public ClickHouseShowRolesCommandBuilder Enabled(bool value = true) { _enabled = value; if (value) _current = false; return this; }
    public ClickHouseShowRolesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ");
        if (_current)
            sb.Append("CURRENT ");
        else if (_enabled)
            sb.Append("ENABLED ");
        sb.Append("ROLES");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
