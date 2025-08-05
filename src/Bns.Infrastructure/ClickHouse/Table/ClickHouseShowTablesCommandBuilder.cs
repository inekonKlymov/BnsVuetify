namespace Bns.Infrastructure.ClickHouse.Table;


public class ClickHouseShowTablesCommandBuilder : ClickHouseCommandBuilder
{
    private bool _full = false;
    private bool _temporary = false;
    private string _fromDb = string.Empty;
    private string _like = string.Empty;
    private bool _notLike = false;
    private string _iLike = string.Empty;
    private int? _limit = null;
    private string _intoOutfile = string.Empty;
    private string _format = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowTablesCommandBuilder Full(bool value = true) { _full = value; return this; }
    public ClickHouseShowTablesCommandBuilder Temporary(bool value = true) { _temporary = value; return this; }
    public ClickHouseShowTablesCommandBuilder From(string db) { _fromDb = db; return this; }
    public ClickHouseShowTablesCommandBuilder Like(string pattern) { _like = pattern; _notLike = false; _iLike = string.Empty; return this; }
    public ClickHouseShowTablesCommandBuilder NotLike(string pattern) { _like = pattern; _notLike = true; _iLike = string.Empty; return this; }
    public ClickHouseShowTablesCommandBuilder ILike(string pattern) { _iLike = pattern; _like = string.Empty; _notLike = false; return this; }
    public ClickHouseShowTablesCommandBuilder Limit(int n) { _limit = n; return this; }
    public ClickHouseShowTablesCommandBuilder IntoOutfile(string filename) { _intoOutfile = filename; return this; }
    public ClickHouseShowTablesCommandBuilder Format(string format) { _format = format; return this; }
    public ClickHouseShowTablesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ");
        if (_full) sb.Append("FULL ");
        if (_temporary) sb.Append("TEMPORARY ");
        sb.Append("TABLES");
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
