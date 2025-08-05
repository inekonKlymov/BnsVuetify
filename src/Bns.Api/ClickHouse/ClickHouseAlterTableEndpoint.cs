using Ardalis.ApiEndpoints;
using Bns.Api.Controllers;
using Bns.Infrastructure.ClickHouse;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.ClickHouse;

public class ClickHouseAlterTableRequest
{
    public string TableName { get; set; } = string.Empty;
    public List<string> AlterActions { get; set; } = new();
    public string? Custom { get; set; }
}


public class ClickHouseAlterTableEndpoint : EndpointBaseAsync
    .WithRequest<ClickHouseAlterTableRequest>
    .WithActionResult<object>
{
    private readonly ClickHouseConnecter _connecter;
    public ClickHouseAlterTableEndpoint(ClickHouseConnecter connecter)
    {
        _connecter = connecter;
    }

    [HttpPost("api/clickhouse/alter-table")]
    [SwaggerOperation(
        Summary = "Alter table in ClickHouse",
        Description = "Alter table using ClickHouseAlterTableCommandBuilder",
        Tags = new[] { ApiExplorerGroups.ClickHouse })
    ]

    public override async Task<ActionResult<object>> HandleAsync([FromBody] ClickHouseAlterTableRequest request, CancellationToken cancellationToken = default)
    {
        var builder = ClickHouseCommandCreator.Columns.Alter()
            .Table(request.TableName);
        foreach (var action in request.AlterActions)
        {
            if (action.StartsWith("ADD COLUMN ")) builder.AddColumn(action[11..]);
            else if (action.StartsWith("DROP COLUMN ")) builder.DropColumn(action[12..]);
            else if (action.StartsWith("MODIFY COLUMN ")) builder.ModifyColumn(action[14..]);
            else builder.Custom(action);
        }
        if (!string.IsNullOrWhiteSpace(request.Custom)) builder.Custom(request.Custom);
        var result = _connecter.ExecuteCommand(builder);
        return Ok(result);
    }
}
