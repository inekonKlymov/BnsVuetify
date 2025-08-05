using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseInsertRowRequest
{
    public string Table { get; set; } = string.Empty;
    public string Columns { get; set; } = string.Empty;
    public List<List<object>> Values { get; set; } = new();
    public string? Custom { get; set; }
}


public class ClickHouseInsertRowEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseInsertRowRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseInsertRowEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/insert")]
    [SwaggerOperation(Summary = "Insert row(s) into ClickHouse", Description = "Insert using ClickHouseInsertRowCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseInsertRowRequest request, CancellationToken cancellationToken = default)
    {
        var builder = ClickHouseCommandCreator.Rows.Insert()
            .Into(request.Table, request.Columns)
            .Values(request.Values.Select(v => v.ToArray()));
        if (!string.IsNullOrWhiteSpace(request.Custom)) builder.Custom(request.Custom);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
