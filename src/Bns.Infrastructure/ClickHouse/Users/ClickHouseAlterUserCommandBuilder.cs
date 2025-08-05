namespace Bns.Infrastructure.ClickHouse.Users;

public class ClickHouseAlterUserCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private readonly List<string> _userNames = new();
    private string _renameTo = string.Empty;
    private string _onCluster = string.Empty;
    private readonly List<string> _authClauses = new();
    private readonly List<string> _addIdentified = new();
    private bool _notIdentified = false;
    private bool _resetAuthMethodsToNew = false;
    private bool _withNoPassword = false;
    private DateTime? _validUntil = null;
    private readonly List<string> _addHosts = new();
    private readonly List<string> _dropHosts = new();
    private bool _anyHost = false;
    private bool _noneHost = false;
    private readonly List<string> _defaultRoles = new();
    private bool _defaultAll = false;
    private readonly List<string> _defaultAllExcept = new();
    private readonly List<string> _grantees = new();
    private bool _granteesAny = false;
    private bool _granteesNone = false;
    private readonly List<string> _exceptGrantees = new();
    private bool _dropAllProfiles = false;
    private bool _dropAllSettings = false;
    private readonly List<string> _dropSettings = new();
    private readonly List<string> _dropProfiles = new();
    private readonly List<string> _addSettings = new();
    private readonly List<string> _modifySettings = new();
    private readonly List<string> _addProfiles = new();
    private string _custom = string.Empty;

    public ClickHouseAlterUserCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterUserCommandBuilder UserNames(params string[] names) { _userNames.AddRange(names); return this; }
    public ClickHouseAlterUserCommandBuilder RenameTo(string newName) { _renameTo = newName; return this; }
    public ClickHouseAlterUserCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterUserCommandBuilder Auth(string clause) { _authClauses.Add(clause); return this; }
    public ClickHouseAlterUserCommandBuilder AddIdentified(string clause) { _addIdentified.Add(clause); return this; }
    public ClickHouseAlterUserCommandBuilder NotIdentified(bool value = true) { _notIdentified = value; return this; }
    public ClickHouseAlterUserCommandBuilder ResetAuthMethodsToNew(bool value = true) { _resetAuthMethodsToNew = value; return this; }
    public ClickHouseAlterUserCommandBuilder WithNoPassword(bool value = true) { _withNoPassword = value; return this; }
    public ClickHouseAlterUserCommandBuilder ValidUntil(DateTime? dt) { _validUntil = dt; return this; }
    public ClickHouseAlterUserCommandBuilder AddHost(string host) { _addHosts.Add(host); return this; }
    public ClickHouseAlterUserCommandBuilder DropHost(string host) { _dropHosts.Add(host); return this; }
    public ClickHouseAlterUserCommandBuilder AnyHost(bool value = true) { _anyHost = value; return this; }
    public ClickHouseAlterUserCommandBuilder NoneHost(bool value = true) { _noneHost = value; return this; }
    public ClickHouseAlterUserCommandBuilder DefaultRole(params string[] roles) { _defaultRoles.AddRange(roles); return this; }
    public ClickHouseAlterUserCommandBuilder DefaultAll(bool value = true) { _defaultAll = value; return this; }
    public ClickHouseAlterUserCommandBuilder DefaultAllExcept(params string[] roles) { _defaultAllExcept.AddRange(roles); return this; }
    public ClickHouseAlterUserCommandBuilder Grantee(params string[] grantees) { _grantees.AddRange(grantees); return this; }
    public ClickHouseAlterUserCommandBuilder GranteesAny(bool value = true) { _granteesAny = value; return this; }
    public ClickHouseAlterUserCommandBuilder GranteesNone(bool value = true) { _granteesNone = value; return this; }
    public ClickHouseAlterUserCommandBuilder ExceptGrantee(params string[] except) { _exceptGrantees.AddRange(except); return this; }
    public ClickHouseAlterUserCommandBuilder DropAllProfiles(bool value = true) { _dropAllProfiles = value; return this; }
    public ClickHouseAlterUserCommandBuilder DropAllSettings(bool value = true) { _dropAllSettings = value; return this; }
    public ClickHouseAlterUserCommandBuilder DropSettings(params string[] settings) { _dropSettings.AddRange(settings); return this; }
    public ClickHouseAlterUserCommandBuilder DropProfiles(params string[] profiles) { _dropProfiles.AddRange(profiles); return this; }
    public ClickHouseAlterUserCommandBuilder AddSettings(params string[] settings) { _addSettings.AddRange(settings); return this; }
    public ClickHouseAlterUserCommandBuilder ModifySettings(params string[] settings) { _modifySettings.AddRange(settings); return this; }
    public ClickHouseAlterUserCommandBuilder AddProfiles(params string[] profiles) { _addProfiles.AddRange(profiles); return this; }
    public ClickHouseAlterUserCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (!_userNames.Any())
            throw new InvalidOperationException("At least one user name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("ALTER USER ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(string.Join(", ", _userNames));
        if (!string.IsNullOrWhiteSpace(_renameTo))
            sb.Append($" RENAME TO {_renameTo}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_notIdentified)
            sb.Append(" NOT IDENTIFIED");
        if (_resetAuthMethodsToNew)
            sb.Append(" RESET AUTHENTICATION METHODS TO NEW");
        if (_authClauses.Any())
            sb.Append(" " + string.Join(", ", _authClauses));
        if (_addIdentified.Any())
            sb.Append(" ADD IDENTIFIED " + string.Join(", ", _addIdentified));
        if (_withNoPassword)
            sb.Append(" WITH NO_PASSWORD");
        if (_validUntil.HasValue)
            sb.Append($" VALID UNTIL '{_validUntil.Value:yyyy-MM-dd HH:mm:ss}'");
        if (_addHosts.Any())
            sb.Append(" ADD HOST " + string.Join(", ", _addHosts));
        if (_dropHosts.Any())
            sb.Append(" DROP HOST " + string.Join(", ", _dropHosts));
        if (_anyHost)
            sb.Append(" ANY");
        if (_noneHost)
            sb.Append(" NONE");
        if (_defaultRoles.Any())
            sb.Append($" DEFAULT ROLE {string.Join(", ", _defaultRoles)}");
        if (_defaultAll)
            sb.Append(" DEFAULT ROLE ALL");
        if (_defaultAllExcept.Any())
            sb.Append($" DEFAULT ROLE ALL EXCEPT {string.Join(", ", _defaultAllExcept)}");
        if (_grantees.Any())
        {
            sb.Append($" GRANTEES {string.Join(", ", _grantees)}");
            if (_exceptGrantees.Any())
                sb.Append($" EXCEPT {string.Join(", ", _exceptGrantees)}");
        }
        if (_granteesAny)
            sb.Append(" GRANTEES ANY");
        if (_granteesNone)
            sb.Append(" GRANTEES NONE");
        if (_dropAllProfiles)
            sb.Append(" DROP ALL PROFILES");
        if (_dropAllSettings)
            sb.Append(" DROP ALL SETTINGS");
        if (_dropSettings.Any())
            sb.Append($" DROP SETTINGS {string.Join(", ", _dropSettings)}");
        if (_dropProfiles.Any())
            sb.Append($" DROP PROFILES {string.Join(", ", _dropProfiles)}");
        if (_addSettings.Any())
            sb.Append($" ADD SETTINGS {string.Join(", ", _addSettings)}");
        if (_modifySettings.Any())
            sb.Append($" MODIFY SETTINGS {string.Join(", ", _modifySettings)}");
        if (_addProfiles.Any())
            sb.Append($" ADD PROFILES {string.Join(", ", _addProfiles)}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
