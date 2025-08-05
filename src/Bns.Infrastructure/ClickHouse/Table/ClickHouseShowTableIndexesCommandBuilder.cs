namespace Bns.Infrastructure.ClickHouse.Table;
public enum ClickHouseShowIndexType
{
    Index,
    Indexes,
    Indices,
    Keys
}

public class ClickHouseShowTableIndexesCommandBuilder : ClickHouseCommandBuilder
{
    private bool _extended = false;
    private ClickHouseShowIndexType _indexType = ClickHouseShowIndexType.Index;
    private string _table = string.Empty;
    private string _fromDb = string.Empty;
    private string _where = string.Empty;
    private string _intoOutfile = string.Empty;
    private string _format = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowTableIndexesCommandBuilder Extended(bool value = true) { _extended = value; return this; }
    public ClickHouseShowTableIndexesCommandBuilder IndexType(ClickHouseShowIndexType type) { _indexType = type; return this; }
    public ClickHouseShowTableIndexesCommandBuilder Table(string table) { _table = table; return this; }
    public ClickHouseShowTableIndexesCommandBuilder FromDb(string db) { _fromDb = db; return this; }
    public ClickHouseShowTableIndexesCommandBuilder Where(string expr) { _where = expr; return this; }
    public ClickHouseShowTableIndexesCommandBuilder IntoOutfile(string filename) { _intoOutfile = filename; return this; }
    public ClickHouseShowTableIndexesCommandBuilder Format(string format) { _format = format; return this; }
    public ClickHouseShowTableIndexesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_table))
            throw new InvalidOperationException("Table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ");
        if (_extended) sb.Append("EXTENDED ");
        sb.Append(_indexType switch
        {
            ClickHouseShowIndexType.Index => "INDEX ",
            ClickHouseShowIndexType.Indexes => "INDEXES ",
            ClickHouseShowIndexType.Indices => "INDICES ",
            ClickHouseShowIndexType.Keys => "KEYS ",
            _ => throw new InvalidOperationException("Unknown index type")
        });
        sb.Append("FROM ");
        sb.Append(_table);
        if (!string.IsNullOrWhiteSpace(_fromDb))
            sb.Append($" FROM {_fromDb}");
        if (!string.IsNullOrWhiteSpace(_where))
            sb.Append($" WHERE {_where}");
        if (!string.IsNullOrWhiteSpace(_intoOutfile))
            sb.Append($" INTO OUTFILE {_intoOutfile}");
        if (!string.IsNullOrWhiteSpace(_format))
            sb.Append($" FORMAT {_format}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
