namespace Bns.Infrastructure.ClickHouse.Settings;

public class ClickHouseShowMergesCommandBuilder : ClickHouseCommandBuilder
{
    private string _like = string.Empty;
    private bool _notLike = false;
    private string _iLike = string.Empty;
    private int? _limit = null;
    private string _custom = string.Empty;

    public ClickHouseShowMergesCommandBuilder Like(string pattern) { _like = pattern; _notLike = false; _iLike = string.Empty; return this; }
    public ClickHouseShowMergesCommandBuilder NotLike(string pattern) { _like = pattern; _notLike = true; _iLike = string.Empty; return this; }
    public ClickHouseShowMergesCommandBuilder ILike(string pattern) { _iLike = pattern; _like = string.Empty; _notLike = false; return this; }
    public ClickHouseShowMergesCommandBuilder Limit(int n) { _limit = n; return this; }
    public ClickHouseShowMergesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW MERGES");
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
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
