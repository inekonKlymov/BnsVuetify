using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;


public class ClickHouseCurrentDatabaseEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseCurrentDatabaseEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/current-database")]
    [SwaggerOperation(Summary = "Get current database in ClickHouse", Description = "Get current database using ClickHouseCurrentDatabaseCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]
    
    public override async Task<ActionResult<object>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseCurrentDatabaseCommandBuilder();
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
