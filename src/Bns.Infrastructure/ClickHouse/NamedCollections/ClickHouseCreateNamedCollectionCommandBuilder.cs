namespace Bns.Infrastructure.ClickHouse.NamedCollections;

public class ClickHouseCreateNamedCollectionCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifNotExists = false;
    private string _name = string.Empty;
    private string _onCluster = string.Empty;
    private readonly List<(string Key, string Value, bool? Overridable)> _items = new();
    private string _custom = string.Empty;

    public ClickHouseCreateNamedCollectionCommandBuilder IfNotExists(bool value = true) { _ifNotExists = value; return this; }
    public ClickHouseCreateNamedCollectionCommandBuilder Name(string name) { _name = name; return this; }
    public ClickHouseCreateNamedCollectionCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseCreateNamedCollectionCommandBuilder AddItem(string key, string value, bool? overridable = null) { _items.Add((key, value, overridable)); return this; }
    public ClickHouseCreateNamedCollectionCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_name) || !_items.Any())
            throw new InvalidOperationException("Collection name and at least one item are required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("CREATE NAMED COLLECTION ");
        if (_ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append(_name);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        sb.Append(" AS\n");
        for (int i = 0; i < _items.Count; i++)
        {
            var (key, value, overridable) = _items[i];
            sb.Append($"{key} = '{value.Replace("'", "''")}'");
            if (overridable.HasValue)
                sb.Append(overridable.Value ? " OVERRIDABLE" : " NOT OVERRIDABLE");
            if (i < _items.Count - 1)
                sb.Append(",\n");
        }
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
