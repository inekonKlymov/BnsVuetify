namespace Bns.Infrastructure.ClickHouse.Dictionaries;

public class ClickHouseDropDictionaryCommandBuilder : ClickHouseCommandBuilder
{
    private string _dictionaryName = string.Empty;
    private bool _ifExists = false;
    private bool _sync = false;
    private string _custom = string.Empty;

    public ClickHouseDropDictionaryCommandBuilder Dictionary(string name) { _dictionaryName = name; return this; }
    public ClickHouseDropDictionaryCommandBuilder IfExists(bool value = true) { _ifExists = value; return this; }
    public ClickHouseDropDictionaryCommandBuilder Sync(bool value = true) { _sync = value; return this; }
    public ClickHouseDropDictionaryCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_dictionaryName))
            throw new InvalidOperationException("Dictionary name is required.");
        var sb = new System.Text.StringBuilder();
        sb.Append("DROP DICTIONARY ");
        if (_ifExists) sb.Append("IF EXISTS ");
        sb.Append(_dictionaryName);
        if (_sync) sb.Append(" SYNC");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
