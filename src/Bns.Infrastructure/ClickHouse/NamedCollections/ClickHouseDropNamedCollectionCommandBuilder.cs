namespace Bns.Infrastructure.ClickHouse.NamedCollections;

public class ClickHouseDropNamedCollectionCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private string _name = string.Empty;
    private string _onCluster = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseDropNamedCollectionCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropNamedCollectionCommandBuilder Name(string name) { _name = name; return this; }
    public ClickHouseDropNamedCollectionCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropNamedCollectionCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Collection name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP NAMED COLLECTION ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_name);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
