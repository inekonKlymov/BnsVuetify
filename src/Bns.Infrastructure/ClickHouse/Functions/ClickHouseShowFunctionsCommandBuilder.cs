namespace Bns.Infrastructure.ClickHouse.Functions;

public class ClickHouseShowFunctionsCommandBuilder : ClickHouseCommandBuilder
{
    private string _like = string.Empty;
    private string _iLike = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowFunctionsCommandBuilder Like(string pattern) { _like = pattern; _iLike = string.Empty; return this; }
    public ClickHouseShowFunctionsCommandBuilder ILike(string pattern) { _iLike = pattern; _like = string.Empty; return this; }
    public ClickHouseShowFunctionsCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW FUNCTIONS");
        if (!string.IsNullOrWhiteSpace(_like))
            sb.Append($" LIKE '{_like}'");
        else if (!string.IsNullOrWhiteSpace(_iLike))
            sb.Append($" ILIKE '{_iLike}'");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
