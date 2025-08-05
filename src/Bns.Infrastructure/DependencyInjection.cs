using Bns.Domain.DataSources;
using Bns.Domain.Users;
using Bns.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data.Common;

namespace Bns.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);
        
        return services;
    }

    public static IServiceProvider InitInfrastructure(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
            SeedData(dbContext);
        }
        return serviceProvider;
    }
    private  static void SeedData(AppDbContext db)
    {
        if (!db.DataSources.Any())
        {
            db.DataSources.Add(new DataSource()
            {
                Id = new DataSourceId(),
                Name = "BNSE-MAMbank",
                Type = DataSourceTypeEnum.MSSQL,
                Enabled = true,
                ConnectionString = "Server=DB42021\\BNSI19;Database=BNSE-MAMbank;Trusted_Connection=True; TrustServerCertificate=True",
                CreateDate = DateTime.UtcNow,
                ModifyDate = DateTime.UtcNow,
            });
        }
        if (!db.Languages.Any())
        {
            db.Languages.AddRange(
            [
                new Language(){Name = "Čeština", Code= "cs"},
                new Language(){Name = "English", Code= "en"}
            ]);
        }
        if (!db.Users.Any())
        {
            //db.Users.Add(
            //    new User(){ UserName = "test",  "pass", new Email("test@inekon.cz"))
            //);
                
        }
        db.SaveChanges();

    } 
}