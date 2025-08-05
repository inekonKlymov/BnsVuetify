namespace Bns.Infrastructure.ClickHouse.Table.Alter.Constraint;

public class ClickHouseAlterTableDropConstraintCommandBuilder : ClickHouseCommandBuilder
{
    private string _tableName = string.Empty;
    private string _onCluster = string.Empty;
    private bool _ifExists = false;
    private string _constraintName = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseAlterTableDropConstraintCommandBuilder Table(string tableName) { _tableName = tableName; return this; }
    public ClickHouseAlterTableDropConstraintCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterTableDropConstraintCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterTableDropConstraintCommandBuilder ConstraintName(string name) { _constraintName = name; return this; }
    public ClickHouseAlterTableDropConstraintCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_tableName) || string.IsNullOrWhiteSpace(_constraintName))
            throw new InvalidOperationException("Table name and constraint name are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append($"ALTER TABLE {_tableName}");
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append(" DROP CONSTRAINT ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_constraintName);
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
