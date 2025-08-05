using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseDropTableRequest
{
    public string TableName { get; set; } = string.Empty;
    public bool IfExists { get; set; } = true;
    public string? Custom { get; set; }
}


public class ClickHouseDropTableEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseDropTableRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseDropTableEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/drop-table")]
    [SwaggerOperation(Summary = "Drop table in ClickHouse", Description = "Drop table using ClickHouseDropTableCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseDropTableRequest request, CancellationToken cancellationToken = default)
    {
        var builder = ClickHouseCommandCreator.Table.Drop()
            .Table(request.TableName)
            .IfExists(request.IfExists);
        if (!string.IsNullOrWhiteSpace(request.Custom)) builder.Custom(request.Custom);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
