using Bns.Domain.Common.Startup;
using DataTables;
using Microsoft.Extensions.Options;
using System.Data.Common;

//using System.Data.Entity;
namespace Bns.Api.Common.Datatables.Backend;

public static class EditorDatabaseExtensions
{
    public static Database RollbackSafely(this Database db) => db.DbTransaction is null ? db : db.Rollback();

    public static Database CreateDatabase(IOptions<AppSettings> _configuration)
    {
        Database result = null;
        try
        {
            result = new Database("postgres",_configuration.Value.ConnectionStrings.DefaultConnection, "Npgsql");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return result;
    }

    public static Database CreateDatabase(string connectionString)
    {
        return new Database("postgres", connectionString, "Npgsql");
    }
}