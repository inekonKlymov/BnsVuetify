namespace Bns.Infrastructure.ClickHouse.Users;

public class ClickHouseShowCreateUserCommandBuilder : ClickHouseCommandBuilder
{
    private readonly List<string> _userNames = new();
    private bool _currentUser = false;
    private string _custom = string.Empty;

    public ClickHouseShowCreateUserCommandBuilder UserNames(params string[] names) { _userNames.AddRange(names); _currentUser = false; return this; }
    public ClickHouseShowCreateUserCommandBuilder CurrentUser(bool value = true) { _currentUser = value; if (value) _userNames.Clear(); return this; }
    public ClickHouseShowCreateUserCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW CREATE USER");
        if (_currentUser)
        {
            sb.Append(" CURRENT_USER");
        }
        else if (_userNames.Any())
        {
            sb.Append($" {string.Join(", ", _userNames)}");
        }
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
