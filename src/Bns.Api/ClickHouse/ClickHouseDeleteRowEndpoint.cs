using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseDeleteRowRequest
{
    public string Table { get; set; } = string.Empty;
    public string? Where { get; set; }
    public string? Custom { get; set; }
}


public class ClickHouseDeleteRowEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseDeleteRowRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseDeleteRowEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/delete")]
    [SwaggerOperation(Summary = "Delete row(s) in ClickHouse", Description = "Delete using ClickHouseDeleteRowCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseDeleteRowRequest request, CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseDeleteRowCommandBuilder()
            .From(request.Table);
        if (!string.IsNullOrWhiteSpace(request.Where)) builder.Where(request.Where);
        if (!string.IsNullOrWhiteSpace(request.Custom)) builder.Custom(request.Custom);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
