using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bns.Infrastructure.ClickHouse.Systems;

public class ClickHouseShowProcesslistCommandBuilder : ClickHouseCommandBuilder
{
    private string _intoOutfile = string.Empty;
    private string _format = string.Empty;
    private string _custom = string.Empty;

    public ClickHouseShowProcesslistCommandBuilder IntoOutfile(string filename) { _intoOutfile = filename; return this; }
    public ClickHouseShowProcesslistCommandBuilder Format(string format) { _format = format; return this; }
    public ClickHouseShowProcesslistCommandBuilder Custom(string sqlPart) { _custom += " " + sqlPart; return this; }

    public override string Build()
    {
        var sb = new StringBuilder();
        sb.Append("SHOW PROCESSLIST");
        if (!string.IsNullOrWhiteSpace(_intoOutfile))
            sb.Append($" INTO OUTFILE {_intoOutfile}");
        if (!string.IsNullOrWhiteSpace(_format))
            sb.Append($" FORMAT {_format}");
        if (!string.IsNullOrWhiteSpace(_custom))
            sb.Append(_custom);
        return sb.ToString();
    }
}
