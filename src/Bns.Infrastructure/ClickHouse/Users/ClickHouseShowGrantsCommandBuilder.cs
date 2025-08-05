namespace Bns.Infrastructure.ClickHouse.Users;

public class ClickHouseShowGrantsCommandBuilder : ClickHouseCommandBuilder
{
    private readonly List<string> _users = new();
    private bool _withImplicit = false;
    private bool _final = false;
    private string _custom = string.Empty;

    public ClickHouseShowGrantsCommandBuilder ForUsers(params string[] users) { _users.AddRange(users); return this; }
    public ClickHouseShowGrantsCommandBuilder WithImplicit(bool value = true) { _withImplicit = value; return this; }
    public ClickHouseShowGrantsCommandBuilder Final(bool value = true) { _final = value; return this; }
    public ClickHouseShowGrantsCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW GRANTS");
        if (_users.Any())
            sb.Append($" FOR {string.Join(", ", _users)}");
        if (_withImplicit)
            sb.Append(" WITH IMPLICIT");
        if (_final)
            sb.Append(" FINAL");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
