using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;


public class ClickHouseShowDatabasesEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseShowDatabasesEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/show-databases")]
    [SwaggerOperation(Summary = "Show databases in ClickHouse", Description = "Show databases using ClickHouseShowDatabasesCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseShowDatabasesCommandBuilder();
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
