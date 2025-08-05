namespace Bns.Infrastructure.ClickHouse.Databases;

public class ClickHouseShowDatabasesCommandBuilder : ClickHouseCommandBuilder
{
    private string _like = string.Empty;
    private bool _notLike = false;
    private string _iLike = string.Empty;
    private int? _limit = null;
    private string _intoOutfile = string.Empty;
    private string _format = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowDatabasesCommandBuilder Like(string pattern) { _like = pattern; _notLike = false; _iLike = string.Empty; return this; }
    public ClickHouseShowDatabasesCommandBuilder NotLike(string pattern) { _like = pattern; _notLike = true; _iLike = string.Empty; return this; }
    public ClickHouseShowDatabasesCommandBuilder ILike(string pattern) { _iLike = pattern; _like = string.Empty; _notLike = false; return this; }
    public ClickHouseShowDatabasesCommandBuilder Limit(int n) { _limit = n; return this; }
    public ClickHouseShowDatabasesCommandBuilder IntoOutfile(string filename) { _intoOutfile = filename; return this; }
    public ClickHouseShowDatabasesCommandBuilder Format(string format) { _format = format; return this; }
    public ClickHouseShowDatabasesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW DATABASES");
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