namespace Bns.Infrastructure.ClickHouse.Views;

public class ClickHouseDropViewCommandBuilder : ClickHouseCommandBuilder
{
    private bool _ifExists = false;
    private string _viewName = string.Empty;
    private string _onCluster = string.Empty;
    private bool _sync = false;
    private string _custom = string.Empty;

    public ClickHouseDropViewCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropViewCommandBuilder View(string name) { _viewName = name; return this; }
    public ClickHouseDropViewCommandBuilder OnCluster(string cluster) { _onCluster = cluster; return this; }
    public ClickHouseDropViewCommandBuilder Sync(bool value = true) { _sync = value; return this; }
    public ClickHouseDropViewCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_viewName))
            throw new InvalidOperationException("View name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP VIEW ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_viewName);
        if (!string.IsNullOrWhiteSpace(_onCluster))
            sb.Append($" ON CLUSTER {_onCluster}");
        if (_sync) sb.Append(" SYNC");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
