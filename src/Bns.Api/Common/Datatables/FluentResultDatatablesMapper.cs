using DataTables;
using FluentResults;

//using System.Data.Entity;
namespace Bns.Api.Common.Datatables;

public static class FluentResultDatatablesMapper
{
    public static readonly string FieldMetadataName = "FieldMetadata";

    public static DtResponse ToDtResponse(this IResultBase result)
    {
        if (result.IsSuccess)
        {
            return new DtResponse();
        }
        else
        {
            var resultreturn = new DtResponse();
            foreach (var err in result.Errors.Where(s => s.Metadata.ContainsKey(FieldMetadataName)))
            {
                resultreturn.fieldErrors.Add(new DtResponse.FieldError() { status = err.Message, name = err.Metadata[FieldMetadataName].ToString() });
            }
            foreach (var err in result.Errors.Where(s => !s.Metadata.ContainsKey(FieldMetadataName)))
            {
                resultreturn.error += "\r\n" + err.Message;
            }
            return resultreturn;
        }
    }

    public static Error AddDtFieldError(this Error error, string dtFieldName)
    {
        return error.WithMetadata(FieldMetadataName, dtFieldName);
    }
}