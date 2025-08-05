using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseDescribeTableRequest
{
    public string TableName { get; set; } = string.Empty;
}


public class ClickHouseDescribeTableEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseDescribeTableRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseDescribeTableEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/describe-table")]
    [SwaggerOperation(Summary = "Describe table in ClickHouse", Description = "Describe table using ClickHouseDescribeTableCommandBuilder", Tags = new[] { ApiExplorerGroups.ClickHouse })]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseDescribeTableRequest request, CancellationToken cancellationToken = default)
    {
        var builder = new ClickHouseDescribeTableCommandBuilder()
            .Table(request.TableName);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
