using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseSystemTableSelectRequest
{
    public string Table { get; set; } = string.Empty;
    public string? Where { get; set; }
    public string? Columns { get; set; }
}


public class ClickHouseSystemTableSelectEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseSystemTableSelectRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseSystemTableSelectEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/system-table-select")]
    [SwaggerOperation(Summary = "Select from system table in ClickHouse", Description = "Select from system table using ClickHouseSystemTableSelectCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseSystemTableSelectRequest request, CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseSystemTableSelectCommandBuilder()
            .Table(request.Table);
        if (!string.IsNullOrWhiteSpace(request.Where)) builder.Where(request.Where);
        if (!string.IsNullOrWhiteSpace(request.Columns)) builder.Columns(request.Columns);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
