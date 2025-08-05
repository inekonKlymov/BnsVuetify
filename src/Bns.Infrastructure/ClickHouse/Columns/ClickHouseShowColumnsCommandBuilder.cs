namespace Bns.Infrastructure.ClickHouse.Columns;

public class ClickHouseShowColumnsCommandBuilder : ClickHouseCommandBuilder
{
    private bool _extended = false;
    private bool _full = false;
    private string _table = string.Empty;
    private string _fromDb = string.Empty;
    private string _like = string.Empty;
    private bool _notLike = false;
    private string _iLike = string.Empty;
    private string _where = string.Empty;
    private int? _limit = null;
    private string _intoOutfile = string.Empty;
    private string _format = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowColumnsCommandBuilder Extended(bool value = true) { _extended = value; return this; }
    public ClickHouseShowColumnsCommandBuilder Full(bool value = true) { _full = value; return this; }
    public ClickHouseShowColumnsCommandBuilder Table(string table) { _table = table; return this; }
    public ClickHouseShowColumnsCommandBuilder FromDb(string db) { _fromDb = db; return this; }
    public ClickHouseShowColumnsCommandBuilder Like(string pattern) { _like = pattern; _notLike = false; _iLike = string.Empty; _where = string.Empty; return this; }
    public ClickHouseShowColumnsCommandBuilder NotLike(string pattern) { _like = pattern; _notLike = true; _iLike = string.Empty; _where = string.Empty; return this; }
    public ClickHouseShowColumnsCommandBuilder ILike(string pattern) { _iLike = pattern; _like = string.Empty; _notLike = false; _where = string.Empty; return this; }
    public ClickHouseShowColumnsCommandBuilder Where(string expr) { _where = expr; _like = string.Empty; _notLike = false; _iLike = string.Empty; return this; }
    public ClickHouseShowColumnsCommandBuilder Limit(int n) { _limit = n; return this; }
    public ClickHouseShowColumnsCommandBuilder IntoOutfile(string filename) { _intoOutfile = filename; return this; }
    public ClickHouseShowColumnsCommandBuilder Format(string format) { _format = format; return this; }
    public ClickHouseShowColumnsCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_table))
            throw new InvalidOperationException("Table name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ");
        if (_extended) sb.Append("EXTENDED ");
        if (_full) sb.Append("FULL ");
        sb.Append("COLUMNS FROM ");
        sb.Append(_table);
        if (!string.IsNullOrWhiteSpace(_fromDb))
            sb.Append($" FROM {_fromDb}");
        if (!string.IsNullOrWhiteSpace(_like))
        {
            sb.Append(_notLike ? " NOT LIKE " : " LIKE ");
            sb.Append($"'{_like}'");
        }
        else if (!string.IsNullOrWhiteSpace(_iLike))
        {
            sb.Append($" ILIKE '{_iLike}'");
        }
        else if (!string.IsNullOrWhiteSpace(_where))
        {
            sb.Append($" WHERE {_where}");
        }
        if (_limit.HasValue)
            sb.Append($" LIMIT {_limit.Value}");
        if (!string.IsNullOrWhiteSpace(_intoOutfile))
            sb.Append($" INTO OUTFILE {_intoOutfile}");
        if (!string.IsNullOrWhiteSpace(_format))
            sb.Append($" FORMAT {_format}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
