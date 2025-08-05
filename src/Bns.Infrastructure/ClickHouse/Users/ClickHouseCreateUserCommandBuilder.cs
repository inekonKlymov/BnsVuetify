namespace Bns.Infrastructure.ClickHouse.Users;

public class ClickHouseCreateUserCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifNotExists = false;
    private bool _orReplace = false;
    private readonly List<string> _userNames = new();
    private string _onCluster = "";
    private readonly List<string> _authClauses = new();
    private readonly List<string> _hosts = new();
    private DateTime? _validUntil = null;
    private string _accessStorageType = "";
    private readonly List<string> _defaultRoles = new();
    private string _defaultDatabase = "";
    private readonly List<string> _grantees = new();
    private readonly List<string> _exceptGrantees = new();
    private readonly List<string> _settings = new();
    private string _custom = "";

    public ClickHouseCreateUserCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseCreateUserCommandBuilder OrReplace(bool value = true) { _orReplace = value; return this; }
    public ClickHouseCreateUserCommandBuilder UserNames(params string[] names) { _userNames.AddRange(names); return this; }
    public ClickHouseCreateUserCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseCreateUserCommandBuilder Authentication(string authClause) { _authClauses.Add(authClause); return this; }
    public ClickHouseCreateUserCommandBuilder Host(string hostClause) { _hosts.Add(hostClause); return this; }
    public ClickHouseCreateUserCommandBuilder ValidUntil(DateTime? dt) { _validUntil = dt; return this; }
    public ClickHouseCreateUserCommandBuilder AccessStorageType(string type) { _accessStorageType = type; return this; }
    public ClickHouseCreateUserCommandBuilder DefaultRole(params string[] roles) { _defaultRoles.AddRange(roles); return this; }
    public ClickHouseCreateUserCommandBuilder DefaultDatabase(string db) { _defaultDatabase = db; return this; }
    public ClickHouseCreateUserCommandBuilder Grantee(params string[] grantees) { _grantees.AddRange(grantees); return this; }
    public ClickHouseCreateUserCommandBuilder ExceptGrantee(params string[] except) { _exceptGrantees.AddRange(except); return this; }
    public ClickHouseCreateUserCommandBuilder Setting(string setting) { _settings.Add(setting); return this; }
    public ClickHouseCreateUserCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_userNames.Any())
            throw new InvalidOperationException("At least one user name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("CREATE ");
        if (_orReplace) sb.Append("OR REPLACE ");
        sb.Append("USER ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append(string.Join(", ", _userNames));
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_authClauses.Any())
            sb.Append(" " + string.Join(", ", _authClauses));
        if (_hosts.Any())
            sb.Append(" HOST " + string.Join(", ", _hosts));
        if (_validUntil.HasValue)
            sb.Append($" VALID UNTIL '{_validUntil.Value:yyyy-MM-dd HH:mm:ss}'");
        if (!string.IsNullOrWhiteSpace(_accessStorageType))
            sb.Append($" IN {_accessStorageType}");
        if (_defaultRoles.Any())
            sb.Append($" DEFAULT ROLE {string.Join(", ", _defaultRoles)}");
        if (!string.IsNullOrWhiteSpace(_defaultDatabase))
            sb.Append($" DEFAULT DATABASE {_defaultDatabase}");
        if (_grantees.Any())
        {
            sb.Append($" GRANTEES {string.Join(", ", _grantees)}");
            if (_exceptGrantees.Any())
                sb.Append($" EXCEPT {string.Join(", ", _exceptGrantees)}");
        }
        if (_settings.Any())
            sb.Append($" SETTINGS {string.Join(", ", _settings)}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
