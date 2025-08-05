namespace Bns.Infrastructure.ClickHouse.Table.Alter;

public class ClickHouseAlterTableSettingsCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private readonly List<string> _actions = new();
    private string _custom = string.Empty;

    public ClickHouseAlterTableSettingsCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableSettingsCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableSettingsCommandBuilder ModifySetting(string setting, string value) { _actions.Add($"MODIFY SETTING {setting} = {value}"); return this; }
    public ClickHouseAlterTableSettingsCommandBuilder ResetSetting(string setting) { _actions.Add($"RESET SETTING {setting}"); return this; }
    public ClickHouseAlterTableSettingsCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || !_actions.Any())
            throw new InvalidOperationException("Table name and at least one action are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" {string.Join(", ", _actions)}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
