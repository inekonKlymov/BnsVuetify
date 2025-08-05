namespace Bns.Infrastructure.ClickHouse.Settings;

public class ClickHouseShowEnginesCommandBuilder : ClickHouseCommandBuilder
{
    private string _intoOutfile = string.Empty;
    private string _format = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowEnginesCommandBuilder IntoOutfile(string filename) { _intoOutfile = filename; return this; }
    public ClickHouseShowEnginesCommandBuilder Format(string format) { _format = format; return this; }
    public ClickHouseShowEnginesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW ENGINES");
        if (!string.IsNullOrWhiteSpace(_intoOutfile))
            sb.Append($" INTO OUTFILE {_intoOutfile}");
        if (!string.IsNullOrWhiteSpace(_format))
            sb.Append($" FORMAT {_format}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
