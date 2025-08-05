namespace Bns.Infrastructure.ClickHouse.Dictionaries;

public class ClickHouseCreateDictionaryCommandBuilder : ClickHouseCommandBuilder
{
    private bool _orReplace = false;
    private bool _ifNotExists = false;
    private string _dictionaryName = "";
    private string _onCluster = "";
    private readonly List<ClickHouseDictionaryFieldDefinition> _fields = new();
    private readonly List<string> _primaryKey = new();
    private string _sourceName = "";
    private readonly Dictionary<string, string> _sourceParams = new();
    private string _layoutName = "";
    private readonly Dictionary<string, string> _layoutParams = new();
    private int? _lifetimeMin = null;
    private int? _lifetimeMax = null;
    private readonly Dictionary<string, string> _settings = new();
    private string _comment = "";
    private string _custom = "";

    public ClickHouseCreateDictionaryCommandBuilder OrReplace(bool orReplace = true) { _orReplace = orReplace; return this; }
    public ClickHouseCreateDictionaryCommandBuilder IfNotExists(bool ifNotExists = true) { _ifNotExists = ifNotExists; return this; }
    public ClickHouseCreateDictionaryCommandBuilder Dictionary(string name) { _dictionaryName = name; return this; }
    public ClickHouseCreateDictionaryCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseCreateDictionaryCommandBuilder AddField(ClickHouseDictionaryFieldDefinition field) { _fields.Add(field); return this; }
    public ClickHouseCreateDictionaryCommandBuilder Fields(IEnumerable<ClickHouseDictionaryFieldDefinition> fields) { _fields.AddRange(fields); return this; }
    public ClickHouseCreateDictionaryCommandBuilder PrimaryKey(params string[] keys) { _primaryKey.Clear(); _primaryKey.AddRange(keys); return this; }
    public ClickHouseCreateDictionaryCommandBuilder Source(string name, Dictionary<string, string>? parameters = null) { _sourceName = name; if (parameters != null) foreach (var p in parameters) _sourceParams[p.Key] = p.Value; return this; }
    public ClickHouseCreateDictionaryCommandBuilder Layout(string name, Dictionary<string, string>? parameters = null) { _layoutName = name; if (parameters != null) foreach (var p in parameters) _layoutParams[p.Key] = p.Value; return this; }
    public ClickHouseCreateDictionaryCommandBuilder Lifetime(int? min = null, int? max = null) { _lifetimeMin = min; _lifetimeMax = max; return this; }
    public ClickHouseCreateDictionaryCommandBuilder Setting(string name, string value) { _settings[name] = value; return this; }
    public ClickHouseCreateDictionaryCommandBuilder Comment(string comment) { _comment = comment; return this; }
    public ClickHouseCreateDictionaryCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_dictionaryName) || !_fields.Any() || !_primaryKey.Any() || string.IsNullOrWhiteSpace(_sourceName) || string.IsNullOrWhiteSpace(_layoutName))
            throw new InvalidOperationException("Dictionary name, fields, primary key, source, and layout are required.");
        var query = "CREATE ";
        if (_orReplace) query += "OR REPLACE ";
        query += "DICTIONARY ";
        if (_ifNotExists) query += "IF NOT EXISTS ";
        query += _dictionaryName;
        if (!string.IsNullOrWhiteSpace(_onCluster)) query += $" ON CLUSTER {_onCluster}";
        query += $" (\n    {string.Join(",\n    ", _fields.Select(f => f.ToString()))}\n)";
        query += $" PRIMARY KEY {string.Join(", ", _primaryKey)}";
        query += $"\nSOURCE({_sourceName}";
        if (_sourceParams.Any())
            query += $"({string.Join(" ", _sourceParams.Select(p => $"{p.Key} {p.Value}"))})";
        query += ")";
        query += $"\nLAYOUT({_layoutName}";
        if (_layoutParams.Any())
            query += $"({string.Join(" ", _layoutParams.Select(p => $"{p.Key} {p.Value}"))})";
        query += ")";
        if (_lifetimeMin.HasValue || _lifetimeMax.HasValue)
        {
            query += "\nLIFETIME(";
            if (_lifetimeMin.HasValue) query += $"MIN {_lifetimeMin.Value} ";
            if (_lifetimeMax.HasValue) query += $"MAX {_lifetimeMax.Value}";
            query = query.TrimEnd();
            query += ")";
        }
        if (_settings.Any())
            query += $"\nSETTINGS({string.Join(", ", _settings.Select(s => $"{s.Key} = {s.Value}"))})";
        if (!string.IsNullOrWhiteSpace(_comment))
            query += $"\nCOMMENT '{_comment.Replace("'", "''")}'";
        if (!string.IsNullOrWhiteSpace(_custom))
            query += _custom;
        return query;
    }
}
