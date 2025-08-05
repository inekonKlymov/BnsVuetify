using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseShowCreateTableRequest
{
    public string TableName { get; set; } = string.Empty;
}


public class ClickHouseShowCreateTableEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseShowCreateTableRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseShowCreateTableEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/show-create-table")]
    [SwaggerOperation(Summary = "Show create table in ClickHouse", Description = "Show create table using ClickHouseShowCreateTableCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseShowCreateTableRequest request, CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseShowCreateTableCommandBuilder()
            .Table(request.TableName);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
