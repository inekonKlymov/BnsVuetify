namespace Bns.Infrastructure.ClickHouse.Rows;

public class ClickHouseInsertRowCommandBuilder : ClickHouseCommandBuilder
{
    private bool _useTableKeyword = false;
    private string _insertTable = "";
    private string _insertColumns = "";
    private readonly List<string> _insertValues = new();
    private string _settings = "";
    private string _custom = "";

    public ClickHouseInsertRowCommandBuilder TableKeyword(bool useTable = true)
    {
        _useTableKeyword = useTable;
        return this;
    }
    public ClickHouseInsertRowCommandBuilder Into(string table, string columns)
    {
        _insertTable = table;
        _insertColumns = columns;
        return this;
    }
    public ClickHouseInsertRowCommandBuilder Values(params object[] values)
    {
        var formatted = string.Join(", ", values.Select(FormatValue));
        _insertValues.Add($"({formatted})");
        return this;
    }
    public ClickHouseInsertRowCommandBuilder Values(IEnumerable<object[]> rows)
    {
        foreach (var row in rows)
            Values(row);
        return this;
    }
    public ClickHouseInsertRowCommandBuilder Settings(string settings)
    {
        _settings = settings;
        return this;
    }
    public ClickHouseInsertRowCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_insertTable) || string.IsNullOrWhiteSpace(_insertColumns) || !_insertValues.Any())
            throw new InvalidOperationException("INSERT INTO, columns, and at least one VALUES row are required.");
        var query = "INSERT INTO ";
        if (_useTableKeyword)
            query += "TABLE ";
        query += $"{_insertTable} ({_insertColumns})";
        if (!string.IsNullOrWhiteSpace(_settings))
            query += $" SETTINGS {_settings}";
        query += $" VALUES {string.Join(", ", _insertValues)}";
        if (!string.IsNullOrWhiteSpace(_custom))
            query += _custom;
        return query;
    }
    private static string FormatValue(object value) => value switch
    {
        null => "NULL",
        string s => $"'{s.Replace("'", "''")}'",
        DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
        bool b => b ? "1" : "0",
        _ => value.ToString()
    };
}
