using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Bns.Infrastructure.ClickHouse.Columns;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;


public class ClickHouseCreateTableRequest
{
    public string TableName { get; set; } = string.Empty;
    public List<ClickHouseColumnDefinition> Columns { get; set; } = [];
    public string? Engine { get; set; }
    public string? OrderBy { get; set; }
    public string? Custom { get; set; }
}



public class ClickHouseCreateTableEndpoint : EndpointBaseAsync
.WithRequest<ClickHouseCreateTableRequest>
.WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseCreateTableEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/create-table")]
    [SwaggerOperation(Summary = "Create table in ClickHouse", Description = "Create table using ClickHouseCreateTableCommandBuilder",
        Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseCreateTableRequest request, CancellationToken cancellationToken = default)
    {
        var builder =  ClickHouseCommandCreator.Table.Create()
            .Table(request.TableName)
            .Columns(request.Columns);
        if (!string.IsNullOrWhiteSpace(request.Engine)) builder.Engine(request.Engine);
        if (!string.IsNullOrWhiteSpace(request.OrderBy)) builder.OrderBy(request.OrderBy);
        if (!string.IsNullOrWhiteSpace(request.Custom)) builder.Custom(request.Custom);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
