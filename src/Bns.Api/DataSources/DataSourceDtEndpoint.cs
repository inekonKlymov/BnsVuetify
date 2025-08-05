using Ardalis.ApiEndpoints;
using Bns.Api.Common.Datatables.Backend;
using Bns.Api.Controllers;
using Bns.Domain.Common.Startup;
using Bns.Infrastructure.Database;
using DataTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Bns.Api.DataSources;

public class DataSourceDtEndpoint(IOptions<AppSettings> configuration, AppDbContext _db) : EndpointBaseAsync.WithoutRequest.WithActionResult<DtResponse>
{
    [Authorize]
    [HttpPost("api/data-sources")]
    [SwaggerOperation(Summary = "Datatables Datasources Admin", Description = "Use datatables Api to get list of datasources", Tags = new[] { ApiExplorerGroups.DataSources})]
    public override async Task<ActionResult<DtResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {

        using var db = EditorDatabaseExtensions.CreateDatabase(configuration);
        var editor = _db.DataSources.CreateEditor(db);
        editor.Field([
            _db.DataSources.CreateDataTablesFieldAuto(s=>s.Id),
                    _db.DataSources.CreateDataTablesFieldAuto(s=>s.Name),
                    _db.DataSources.CreateDataTablesFieldAuto(s=>s.Type)
                        //.Options(
                        //    _db.ItemType.DbSetOptions(
                        //        s=>s.Name,
                        //        s=>s.Name,
                        //        s=>s.DbSetWhere(_db.ItemType,s=>s.Enabled,true).DbSetAndWhere(_db.ItemType, s=>s.Area,"DataSources")
                        //))
                        ,
                    _db.DataSources.CreateDataTablesFieldAuto(s=>s.ConnectionString),
                    _db.DataSources.CreateDataTablesFieldAuto(s=>s.Enabled),
                    _db.DataSources.CreateDataTablesFieldAuto(s=>s.ConfigurationId)
                        //.Type<MSOLAPConfig>()
                        //.GetFormatter(EditorFieldExtensions.GetFormatters.XmlFormatter<MSOLAPConfig>())
                        //.SetFormatter(EditorFieldExtensions.SetFormatters.XmlFormatter<MSOLAPConfig>())
                        ,
                    _db.DataSources.CreateDataTablesFieldAuto(s=>s.CreateDate)
                        .Set(Field.SetType.Create),
                    _db.DataSources.CreateDataTablesFieldAuto(s=>s.ModifyDate)
                        .Set(Field.SetType.Both)
        ]);
        editor.PreCreate += (sender, e) =>
        {
            editor.Field(_db.DataSources.GetAutoFieldName(s => s.CreateDate)).SetValue(DateTime.Now);
            editor.Field(_db.DataSources.GetAutoFieldName(s => s.ModifyDate)).SetValue(DateTime.Now);
        };
        editor.PreEdit += (sender, e) => editor.Field(_db.DataSources, s => s.ModifyDate).SetValue(DateTime.Now);
        //editor.PostGet += PostGetAddLinks;
        DtResponse response = editor.Process(Request).Data();
        return new JsonResult(response);
    }
}