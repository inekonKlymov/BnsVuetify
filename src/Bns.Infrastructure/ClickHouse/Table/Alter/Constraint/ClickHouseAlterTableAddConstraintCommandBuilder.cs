namespace Bns.Infrastructure.ClickHouse.Table.Alter.Constraint;

public class ClickHouseAlterTableAddConstraintCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private bool _ifNotExists = false;
    private string _constraintName = string.Empty;
    private string _checkExpression = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterTableAddConstraintCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableAddConstraintCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableAddConstraintCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseAlterTableAddConstraintCommandBuilder ConstraintName(string name) { _constraintName = name; return this; }
    public ClickHouseAlterTableAddConstraintCommandBuilder Check(string expr) { _checkExpression = expr; return this; }
    public ClickHouseAlterTableAddConstraintCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_constraintName) || string.IsNullOrWhiteSpace(_checkExpression))
            throw new InvalidOperationException("Table name, constraint name, and check expression are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append(" ADD CONSTRAINT ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append($"{_constraintName} CHECK {_checkExpression}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
