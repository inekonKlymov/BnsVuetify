namespace Bns.Infrastructure.ClickHouse.Rows;

public class ClickHouseSelectRowCommandBuilder : ClickHouseCommandBuilder
{
    private string _select = "*";
    private string _from = "";
    private string _where = "";
    private string _orderBy = "";
    private string _groupBy = "";
    private string _having = "";
    private string _join = "";
    private string _custom = "";
    private int? _limit = null;
    private string _with = "";
    private bool _distinct = false;
    private string _prewhere = "";
    private string _sample = "";
    private string _arrayJoin = "";
    private string _limitBy = "";
    private int? _offset = null;
    private string _union = "";
    private string _settings = "";
    private string _distinctOn = "";
    private bool _final = false;
    private string _joinModifiers = "";
    private string _groupByModifiers = ""; // WITH ROLLUP, WITH CUBE, WITH TOTALS
    private string _window = "";
    private string _qualify = "";
    private string _orderByModifiers = ""; // WITH FILL, FROM, TO, STEP, INTERPOLATE
    private bool _withTies = false;
    private string _intoOutfile = "";
    private string _format = "";

    public ClickHouseSelectRowCommandBuilder Select(string columns)
    {
        _select = columns;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder From(string table)
    {
        _from = table;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Where(string condition)
    {
        _where = condition;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder OrderBy(string orderBy)
    {
        _orderBy = orderBy;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder GroupBy(string groupBy)
    {
        _groupBy = groupBy;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Having(string having)
    {
        _having = having;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Join(string joinClause)
    {
        _join += " " + joinClause;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Custom(string sqlPart)
    {
        _custom += " " + sqlPart;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Limit(int limit)
    {
        _limit = limit;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder With(string withClause)
    {
        _with = withClause;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Distinct(bool distinct = true)
    {
        _distinct = distinct;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Prewhere(string prewhere)
    {
        _prewhere = prewhere;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Sample(string sample)
    {
        _sample = sample;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder ArrayJoin(string arrayJoin)
    {
        _arrayJoin = arrayJoin;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder LimitBy(string limitBy)
    {
        _limitBy = limitBy;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Offset(int offset)
    {
        _offset = offset;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Union(string unionClause)
    {
        _union = unionClause;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Settings(string settings)
    {
        _settings = settings;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder DistinctOn(string columns)
    {
        _distinctOn = columns;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Final(bool final = true)
    {
        _final = final;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder JoinModifiers(string modifiers)
    {
        _joinModifiers = modifiers;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder GroupByModifiers(string modifiers)
    {
        _groupByModifiers = modifiers;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Window(string window)
    {
        _window = window;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Qualify(string qualify)
    {
        _qualify = qualify;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder OrderByModifiers(string modifiers)
    {
        _orderByModifiers = modifiers;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder WithTies(bool withTies = true)
    {
        _withTies = withTies;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder IntoOutfile(string outfile)
    {
        _intoOutfile = outfile;
        return this;
    }
    public ClickHouseSelectRowCommandBuilder Format(string format)
    {
        _format = format;
        return this;
    }

    public override string Build()
    {
        if (string.IsNullOrWhiteSpace(_from))
            throw new InvalidOperationException("FROM clause is required.");
        var query = "";
        if (!string.IsNullOrWhiteSpace(_with))
            query += $"WITH {_with} ";
        query += "SELECT ";
        if (_distinct)
            query += "DISTINCT ";
        if (!string.IsNullOrWhiteSpace(_distinctOn))
            query += $"ON ({_distinctOn}) ";
        query += _select;
        query += $" FROM {_from}";
        if (_final)
            query += " FINAL";
        if (!string.IsNullOrWhiteSpace(_sample))
            query += $" SAMPLE {_sample}";
        if (!string.IsNullOrWhiteSpace(_arrayJoin))
            query += $" ARRAY JOIN {_arrayJoin}";
        if (!string.IsNullOrWhiteSpace(_joinModifiers))
            query += $" {_joinModifiers}";
        if (!string.IsNullOrWhiteSpace(_join))
            query += _join;
        if (!string.IsNullOrWhiteSpace(_prewhere))
            query += $" PREWHERE {_prewhere}";
        if (!string.IsNullOrWhiteSpace(_where))
            query += $" WHERE {_where}";
        if (!string.IsNullOrWhiteSpace(_groupBy))
            query += $" GROUP BY {_groupBy}";
        if (!string.IsNullOrWhiteSpace(_groupByModifiers))
            query += $" {_groupByModifiers}";
        if (!string.IsNullOrWhiteSpace(_having))
            query += $" HAVING {_having}";
        if (!string.IsNullOrWhiteSpace(_window))
            query += $" WINDOW {_window}";
        if (!string.IsNullOrWhiteSpace(_qualify))
            query += $" QUALIFY {_qualify}";
        if (!string.IsNullOrWhiteSpace(_orderBy))
            query += $" ORDER BY {_orderBy}";
        if (!string.IsNullOrWhiteSpace(_orderByModifiers))
            query += $" {_orderByModifiers}";
        if (!string.IsNullOrWhiteSpace(_limitBy))
            query += $" LIMIT {_limitBy}";
        if (_limit.HasValue)
            query += $" LIMIT {_limit.Value}";
        if (_offset.HasValue)
            query += $" OFFSET {_offset.Value}";
        if (_withTies)
            query += " WITH TIES";
        if (!string.IsNullOrWhiteSpace(_union))
            query += $" UNION {_union}";
        if (!string.IsNullOrWhiteSpace(_settings))
            query += $" SETTINGS {_settings}";
        if (!string.IsNullOrWhiteSpace(_intoOutfile))
            query += $" INTO OUTFILE {_intoOutfile}";
        if (!string.IsNullOrWhiteSpace(_format))
            query += $" FORMAT {_format}";
        if (!string.IsNullOrWhiteSpace(_custom))
            query += _custom;
        return query;
    }
}
