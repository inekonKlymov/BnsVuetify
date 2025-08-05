using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseUpdateRowRequest
{
    public string Table { get; set; } = string.Empty;
    public string SetClause { get; set; } = string.Empty;
    public string? Where { get; set; }
    public string? Custom { get; set; }
}


public class ClickHouseUpdateRowEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseUpdateRowRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseUpdateRowEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/update")]
    [SwaggerOperation(Summary = "Update row(s) in ClickHouse", Description = "Update using ClickHouseUpdateRowCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]
    
    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseUpdateRowRequest request, CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseUpdateRowCommandBuilder()
            .Table(request.Table)
            .Set(request.SetClause);
        if (!string.IsNullOrWhiteSpace(request.Where)) builder.Where(request.Where);
        if (!string.IsNullOrWhiteSpace(request.Custom)) builder.Custom(request.Custom);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
