namespace Bns.Infrastructure.ClickHouse.Dictionaries;

public class ClickHouseShowDictionariesCommandBuilder : ClickHouseCommandBuilder
{
    private string _fromDb = string.Empty;
    private string _like = string.Empty;
    private int? _limit = null;
    private string _intoOutfile = string.Empty;
    private string _format = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowDictionariesCommandBuilder From(string db) { _fromDb = db; return this; }
    public ClickHouseShowDictionariesCommandBuilder Like(string pattern) { _like = pattern; return this; }
    public ClickHouseShowDictionariesCommandBuilder Limit(int n) { _limit = n; return this; }
    public ClickHouseShowDictionariesCommandBuilder IntoOutfile(string filename) { _intoOutfile = filename; return this; }
    public ClickHouseShowDictionariesCommandBuilder Format(string format) { _format = format; return this; }
    public ClickHouseShowDictionariesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW DICTIONARIES");
        if (!string.IsNullOrWhiteSpace(_fromDb))
            sb.Append($" FROM {_fromDb}");
        if (!string.IsNullOrWhiteSpace(_like))
            sb.Append($" LIKE '{_like}'");
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
