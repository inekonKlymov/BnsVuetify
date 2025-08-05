namespace Bns.Infrastructure.ClickHouse.Settings;

public class ClickHouseShowFilesystemCachesCommandBuilder : ClickHouseCommandBuilder
{
    private string _custom = string.Empty;

    public ClickHouseShowFilesystemCachesCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SHOW FILESYSTEM CACHES");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
