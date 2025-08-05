namespace Bns.Infrastructure.ClickHouse.NamedCollections;

public class ClickHouseAlterNamedCollectionCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private string _name = string.Empty;
    private string _onCluster = string.Empty;
    private readonly List<(string Key, string Value, bool? Overridable)> _setItems = new();
    private readonly List<string> _deleteKeys = new();
    private string _custom = string.Empty;

    public ClickHouseAlterNamedCollectionCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseAlterNamedCollectionCommandBuilder Name(string name) { _name = name; return this; }
    public ClickHouseAlterNamedCollectionCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseAlterNamedCollectionCommandBuilder SetItem(string key, string value, bool? overridable = null) { _setItems.Add((key, value, overridable)); return this; }
    public ClickHouseAlterNamedCollectionCommandBuilder DeleteKey(string key) { _deleteKeys.Add(key); return this; }
    public ClickHouseAlterNamedCollectionCommandBuilder DeleteKeys(params string[] keys) { _deleteKeys.AddRange(keys); return this; }
    public ClickHouseAlterNamedCollectionCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Collection name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("ALTER NAMED COLLECTION ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_name);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_setItems.Any())
        {
            sb.Append(" SET\n");
            for (int i = 0; i < _setItems.Count; i++)
            {
                var (key, value, overridable) = _setItems[i];
                sb.Append($"{key} = '{value.Replace("'", "''")}'");
                if (overridable.HasValue)
                    sb.Append(overridable.Value ? " OVERRIDABLE" : " NOT OVERRIDABLE");
                if (i < _setItems.Count - 1)
                    sb.Append(",\n");
            }
        }
        if (_deleteKeys.Any())
        {
            if (_setItems.Any())
                sb.Append("\n");
            sb.Append($"DELETE {string.Join(", ", _deleteKeys)}");
        }
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
