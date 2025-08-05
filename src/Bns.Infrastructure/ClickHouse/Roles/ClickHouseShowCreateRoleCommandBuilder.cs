namespace Bns.Infrastructure.ClickHouse.Roles;

public class ClickHouseShowCreateRoleCommandBuilder : ClickHouseCommandBuilder
{
    private readonly List<string> _roleNames = new();
    private string _custom = string.Empty;

    public ClickHouseShowCreateRoleCommandBuilder RoleNames(params string[] names) { _roleNames.AddRange(names); return this; }
    public ClickHouseShowCreateRoleCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_roleNames.Any())
            throw new InvalidOperationException("At least one role name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW CREATE ROLE ");
        sb.Append(string.Join(", ", _roleNames));
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
