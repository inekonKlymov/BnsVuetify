using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseSelectRowRequest
{
    public string Select { get; set; } = "*";
    public string From { get; set; } = string.Empty;
    public string? Where { get; set; }
    public string? OrderBy { get; set; }
    public string? GroupBy { get; set; }
    public string? Having { get; set; }
    public string? Join { get; set; }
    public string? Custom { get; set; }
    public int? Limit { get; set; }
}


public class ClickHouseSelectRowEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseSelectRowRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseSelectRowEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/select")]
    [SwaggerOperation(Summary = "Select rows from ClickHouse", Description = "Select rows using ClickHouseSelectRowCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseSelectRowRequest request, CancellationToken cancellationToken = default)
    {
        var builder = ClickHouseCommandCreator.Rows.Select()
            .Select(request.Select)
            .From(request.From);
        if (!string.IsNullOrWhiteSpace(request.Where)) builder.Where(request.Where);
        if (!string.IsNullOrWhiteSpace(request.OrderBy)) builder.OrderBy(request.OrderBy);
        if (!string.IsNullOrWhiteSpace(request.GroupBy)) builder.GroupBy(request.GroupBy);
        if (!string.IsNullOrWhiteSpace(request.Having)) builder.Having(request.Having);
        if (!string.IsNullOrWhiteSpace(request.Join)) builder.Join(request.Join);
        if (!string.IsNullOrWhiteSpace(request.Custom)) builder.Custom(request.Custom);
        if (request.Limit.HasValue) builder.Limit(request.Limit.Value);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
