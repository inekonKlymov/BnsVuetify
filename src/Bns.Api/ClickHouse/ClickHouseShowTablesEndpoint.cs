using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseShowTablesRequest
{
    public string? Database { get; set; }
}


public class ClickHouseShowTablesEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseShowTablesRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseShowTablesEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/show-tables")]
    [SwaggerOperation(Summary = "Show tables in ClickHouse", Description = "Show tables using ClickHouseShowTablesCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseShowTablesRequest request, CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseShowTablesCommandBuilder();
        if (!string.IsNullOrWhiteSpace(request.Database)) builder.FromDatabase(request.Database);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
