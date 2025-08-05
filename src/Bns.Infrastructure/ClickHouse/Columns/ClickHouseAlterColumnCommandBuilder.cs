namespace Bns.Infrastructure.ClickHouse.Columns;

public class ClickHouseAlterColumnCommandBuilder : ClickHouseCommandBuilder
{
    private bool _temporary = false;
    private string _tableName = "";
    private string _onCluster = "";
    private readonly List<string> _alterActions = new();
    private string _custom = "";
    public ClickHouseAlterColumnCommandBuilder Temporary(bool value = true)
    {
        _temporary = value;
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder Table(string tableName)
    {
        _tableName = tableName;
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder OnCluster(string cluster)
    {
        _onCluster = cluster;
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder AddAction(string action)
    {
        _alterActions.Add(action);
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder AddColumn(string columnDef)
    {
        _alterActions.Add($"ADD COLUMN {columnDef}");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder DropColumn(string columnName)
    {
        _alterActions.Add($"DROP COLUMN {columnName}");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder RenameColumn(string oldName, string newName)
    {
        _alterActions.Add($"RENAME COLUMN {oldName} TO {newName}");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder ClearColumn(string columnName)
    {
        _alterActions.Add($"CLEAR COLUMN {columnName}");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder CommentColumn(string columnName, string comment)
    {
        _alterActions.Add($"COMMENT COLUMN {columnName} '{comment.Replace("'", "''")}'");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder ModifyColumn(string columnDef)
    {
        _alterActions.Add($"MODIFY COLUMN {columnDef}");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder AlterColumn(string columnDef)
    {
        _alterActions.Add($"ALTER COLUMN {columnDef}");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder MaterializeColumn(string columnName)
    {
        _alterActions.Add($"MATERIALIZE COLUMN {columnName}");
        return this;
    }
    public ClickHouseAlterColumnCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || !_alterActions.Any())
            throw new InvalidOperationException("Table name and at least one alter action are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("ALTER ");
        if (_temporary) sb.Append("TEMPORARY ");
        sb.Append($"TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append($" {string.Join(", ", _alterActions)}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
